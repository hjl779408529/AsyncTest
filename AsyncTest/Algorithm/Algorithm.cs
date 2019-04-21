using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncTest
{
    partial class MainForm 
    {
        //公共变量 
        CancellationTokenSource _Source;
        CancellationToken _Token;
        int ProcessValueMax = 10000;//进度最大值
        int StepValue = 10;//步进值
        int RepeatTimes = 0;//重复次数

        /*****************方法1******************/
        //方法1变量
        /// <summary>
        /// 方法1 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Fun01_Start(object sender, EventArgs e)
        {
            Start_button.Enabled = false;
            _Source = new CancellationTokenSource();
            _Token = _Source.Token;
            Status_progressBar.Maximum = ProcessValueMax;
            RepeatTimes = ProcessValueMax / StepValue;
            for (int i = 0;i< RepeatTimes;i++)
            {
                if (_Token.IsCancellationRequested) break;
                try
                {
                    await Task.Delay(50, _Token);
                }
                catch (Exception)
                {
                    Status_progressBar.Value = i * StepValue;
                }
                finally
                {
                    Status_progressBar.Value = (i + 1) * StepValue;
                }
                
            }
            var msg = _Token.IsCancellationRequested ? $"当前进度:{((float)Status_progressBar.Value / ProcessValueMax) * 100}%" : "完成";
            MessageBox.Show(msg,@"信息");
            StatusRecover();
        }
        /// <summary>
        /// 方法1 终止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Fun01_End(object sender, EventArgs e)
        {
            if (Start_button.Enabled) return;//未在运行中
            Cancel_button.Enabled = false;
            _Source.Cancel();
        }
        /**************公用方法***************/
        /// <summary>
        /// 状态恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StatusRecover()
        {
            this.Invoke((EventHandler)delegate
            {
                Start_button.Enabled = true;
                Cancel_button.Enabled = true;
                Status_progressBar.Value = 0;
            });           
        }

        /*****************方法2******************/
        /// <summary>
        /// 方法二
        /// </summary>
        public void Fun2()
        {
            Status_progressBar.Maximum = ProcessValueMax;
            RepeatTimes = ProcessValueMax / StepValue;
            Start_button.Click += async (sender, e) =>
            {
                _Source = new CancellationTokenSource();
                _Token = _Source.Token;
                Start_button.Enabled = false;
                for (int i = 0; i < RepeatTimes; i++)
                {
                    if (_Token.IsCancellationRequested) break;
                    try
                    {
                        await Task.Delay(50, _Token);
                    }
                    catch (Exception)
                    {
                        Status_progressBar.Value = i * StepValue;
                    }
                    finally
                    {
                        Status_progressBar.Value = (i + 1) * StepValue;
                    }
                }
                var msg = _Token.IsCancellationRequested ? $"当前进度:{((float)Status_progressBar.Value / ProcessValueMax) * 100}%" : "完成";
                MessageBox.Show(msg, @"信息");
                StatusRecover();
            };
            Cancel_button.Click += (sender, e) =>
            {
                if (Start_button.Enabled) return;//未在运行中
                Cancel_button.Enabled = false;
                _Source.Cancel();
            };
        }

        /*****************方法3******************/
        /// <summary>
        /// 方法三
        /// </summary>
        public void Fun3()
        {
            Status_progressBar.Maximum = ProcessValueMax;
            Start_button.Click += (sender, e) =>
            {
                if (!Start_button.Enabled) return;
                _Source = new CancellationTokenSource();
                _Token = _Source.Token;
                Start_button.Enabled = false;
                Thread thread = new Thread(()=>
                {
                    var t = TestYield(ProcessValueMax);
                    MessageBox.Show($"计数值{Loop(10000)}", @"信息");
                    MessageBox.Show($"计数值{Loop(10001)}", @"信息");
                    MessageBox.Show($"计数值{Loop(10002)}", @"信息");
                    MessageBox.Show($"计数值{Loop(10003)}", @"信息");
                    MessageBox.Show($"计数值{Loop(10004)}", @"信息");
                    MessageBox.Show($"计数值{t.Result}", @"信息");
                });
                thread.Start();
            };
            Cancel_button.Click += (sender, e) =>
            {
                if (Start_button.Enabled) return;//未在运行中
                Cancel_button.Enabled = false;
                _Source.Cancel();
            };
        }
        /// <summary>
        /// 循环计数
        /// </summary>
        /// <param name="indata"></param>
        public int Loop(int indata)
        {
            for (int i = 0; i < indata; i++);
            return indata;
        }
        /// <summary>
        /// 方法3 测试Task.Yield
        /// </summary>
        /// <param name="indata"></param>
        /// <returns></returns>
        public async Task<int> TestYield(int indata)
        {
            for (int i = 0;i< indata; i++)
            {
                if (_Token.IsCancellationRequested) break;
                if (i % 1000 == 0)
                {
                    await Task.Yield();
                }
                this.Invoke((EventHandler)delegate
                {
                    Status_progressBar.Value = (i / StepValue) * StepValue;
                });
            }
            var msg = _Token.IsCancellationRequested ? $"当前进度:{((float)Status_progressBar.Value / ProcessValueMax) * 100}%" : "完成";
            MessageBox.Show(msg, @"信息");
            StatusRecover();
            return indata;
        }

        /*****************方法4******************/
        //方法四变量
        private readonly BackgroundWorker _Worker = new BackgroundWorker();


        /// <summary>
        /// 方法四
        /// </summary>
        public void Fun4()
        {
            
            //设置 BackgroundWorker 属性
            _Worker.WorkerReportsProgress = true;   //能否报告进度更新
            _Worker.WorkerSupportsCancellation = true;  //是否支持异步取消

            //绑定_worker处理事件
            _Worker.DoWork += _Worker_DoWork;   //开始执行后台操作时触发，即调用 BackgroundWorker.RunWorkerAsync 时触发
            _Worker.ProgressChanged += _Worker_ProgressChanged; //调用 BackgroundWorker.ReportProgress(System.Int32) 时触发
            _Worker.RunWorkerCompleted += _Worker_RunWorkerCompleted;   //当后台操作已完成、被取消或引发异常时触发

            //绑定按钮事件
            Start_button.Click += Start_Button_Click;
            Cancel_button.Click += Cancel_Button_Click;
        }
        /// <summary>
        /// 当后台操作已完成、被取消或引发异常时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(e.Cancelled ? $@"进程已被取消：{Status_progressBar.Value}%" : $@"进程执行完成：{Status_progressBar.Value}%");
            StatusRecover();
        }

        /// <summary>
        /// 调用 BackgroundWorker.ReportProgress(System.Int32) 时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Status_progressBar.Value = e.ProgressPercentage;   //异步任务的进度百分比
        }

        /// <summary>
        /// 开始执行后台操作触发，即调用 BackgroundWorker.RunWorkerAsync 时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            if (worker == null)
            {
                return;
            }
            for (var i = 0; i < 10; i++)
            {
                //判断程序是否已请求取消后台操作
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                worker.ReportProgress((i + 1) * 10);    //触发 BackgroundWorker.ProgressChanged 事件
                Thread.Sleep(250);  //线程挂起 250 毫秒
            }
        }
        /// <summary>
        /// 启动按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Button_Click(object sender, EventArgs e)
        {
            Start_button.Enabled = false;
            if (!_Worker.IsBusy)
            {
                _Worker.RunWorkerAsync();
            }
        }
        /// <summary>
        /// 终止按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            if (Start_button.Enabled) return;//未在运行中
            _Worker.CancelAsync();

        }


    }
}
