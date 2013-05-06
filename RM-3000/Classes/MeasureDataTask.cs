using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CommonLib;
using DataCommon;

namespace RM_3000.Classes
{
    /// <summary>
    /// 測定データ収集タスク
    /// </summary>
    public class MeasureDataTask
    {
        /// <summary>
        /// 測定データ受信デリゲート
        /// </summary>
        /// <param name="dataList">測定データ</param>
        public delegate void DataReceivedDelegate(SampleData[] dataList);
        /// <summary>
        /// 測定データ受信イベント
        /// </summary>
        public event DataReceivedDelegate DataReceived = null;
        /// <summary>
        /// 状態通知デリゲート
        /// </summary>
        /// <param name="MeasurePause"></param>
        public delegate void GotConditionDelegate(bool bCond_MeasurePause);
        /// <summary>
        /// 状態通知イベント
        /// </summary>
        public event GotConditionDelegate GotCondition = null;

        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 監視周期 [ms]
        /// </summary>
        private const int CycleMilliseconds = 300;
        /// <summary>
        /// タスク
        /// </summary>
        private Task task = null;
        /// <summary>
        /// タスク終了通知イベント
        /// </summary>
        private ManualResetEvent stopTaskEvent = new ManualResetEvent(false);
        /// <summary>
        /// タスク周期計測タイマー
        /// </summary>
        private System.Diagnostics.Stopwatch swTask = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public MeasureDataTask(LogManager log)
        {
            this.log = log;
        }

        #region Public Methods
        /// <summary>
        /// タスク開始
        /// </summary>
        /// <returns>true:成功 / false:失敗</returns>
        public bool Start()
        {
            var ret = false;

            try
            {
                if (this.task == null)
                {
                    // タスク初期化

                    // タスク起動
                    this.task = Task.Factory.StartNew(() => { this.TaskMainMethod(); });
                }

                this.swTask.Reset();
                this.swTask.Start();

                ret = true;
            }
            catch (Exception ex)
            {
                PutErrorLog(ex);
            }

            return ret;
        }
        /// <summary>
        /// タスク停止
        /// </summary>
        /// <returns>true:成功 / false:失敗</returns>
        public bool Stop()
        {
            var ret = false;

            try
            {
                this.swTask.Stop();

                if (this.task != null)
                {
                    this.stopTaskEvent.Set();
                    this.task.Wait();

                }
                ret = true;
            }
            catch (Exception ex)
            {
                PutErrorLog(ex);
            }

            return ret;
        }
        /// <summary>
        /// タスク一時停止
        /// </summary>
        public void Pause()
        {
            this.swTask.Stop();
        }
        #endregion

        #region private method
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void PutErrorLog(Exception ex)
        {
            var message = string.Format("{0}\n{1}", ex.Message, ex.StackTrace);
            if (this.log != null) this.log.PutErrorLog(message);
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void PutErrorLog(string message)
        {
            if (this.log != null) this.log.PutErrorLog(message);
        }

        /// <summary>
        /// タスク終了通知イベントが発生したか
        /// </summary>
        protected bool StopEventHasOcccurred { get { return this.stopTaskEvent.WaitOne(0); } }
        /// <summary>
        /// タスクメイン処理
        /// </summary>
        protected void TaskMainMethod()
        {

            // タスクメインループ
            while (!this.StopEventHasOcccurred)
            {
                try
                {
                    if (this.swTask.ElapsedMilliseconds > CycleMilliseconds)
                    {
                        this.swTask.Reset();
                        this.swTask.Start();

                        GetCondition();

                        CollectMeasureData();
                    }
                }
                catch (Exception ex)
                {
                    this.PutErrorLog("OpcClientTask.TaskMainMethod()\n" + ex.Message + "\n" + ex.StackTrace);
                }

                Thread.Sleep(1);
            }

            // 終了処理


        }

        /// <summary>
        /// 状態取得
        /// </summary>
        private void GetCondition()
        {
            OnGotCondition(RealTimeData.bCond_MeasurePause);
        }

        /// <summary>
        /// 状態取得イベントを発行する
        /// </summary>
        /// <param name="bCond_MeasurePause"></param>
        private void OnGotCondition(bool bCond_MeasurePause)
        {
            if (this.GotCondition != null)
            {
                this.GotCondition(bCond_MeasurePause);
            }
        }

        /// <summary>
        /// 測定データを収集する
        /// </summary>
        private void CollectMeasureData()
        {
            var data = RealTimeData.GetRealTimeDatas();
            if (data != null && data.Count > 0)
            {
                OnDataReceived(data.ToArray());
            }
        }
        /// <summary>
        /// 測定データ受信イベントを発行する
        /// </summary>
        /// <param name="dataList">測定データ</param>
        private void OnDataReceived(SampleData[] dataList)
        {
            if (this.DataReceived != null)
            {
                this.DataReceived(dataList);
            }
        }
        #endregion

    
    }
}
