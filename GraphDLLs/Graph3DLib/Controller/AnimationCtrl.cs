using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using Graph3DLib.Model;

namespace Graph3DLib.Controller
{
    /// <summary>
    /// enum Animation status
    /// </summary>
    public enum AnimationStatus
    {
        Stop = 0,
        Start = 1,
        Pause = 2,
        Resume = 3,
    }

    /// <summary>
    /// animation control class
    /// </summary>
    public class AnimationCtrl
    {
        #region Private variable
        /// <summary>
        /// animation clock 
        /// </summary>
        private AnimationClock[][] _AnimationClock;
        /// <summary>
        /// machine model class
        /// </summary>
        private MachineModel _MachineModel;
        /// <summary>
        /// check is pause
        /// </summary>
        private bool _IsPause;
        /// <summary>
        /// Animation Status
        /// </summary>
        private AnimationStatus _AnimationStatus = AnimationStatus.Stop;
        /// <summary>
        /// Initital TargetType
        /// </summary>
        private TargetType _TargetBase;
        /// <summary>
        /// Initital Keyframe
        /// </summary>
        private AnimationKeyFrame _KeyFrameTypeBase;
        #endregion

        #region Delegate
        /// <summary>
        /// delegate AnimationCompletedEventHandler
        /// </summary>
        /// <param name="currentLine">current line data</param>
        public delegate void AnimationCompletedEventHandler(double duration);
        /// <summary>
        /// event Animation Completed 
        /// </summary>
        public event AnimationCompletedEventHandler AnimationCompleted;
        #endregion

        #region Public Properties

        /// <summary>
        /// get animation current time
        /// </summary>
        public TimeSpan CurrentTime
        {
            get
            {
                TimeSpan outp = new TimeSpan();
                try
                {

                    if (_AnimationClock != null)
                        if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase] != null)
                            if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Controller.Clock.CurrentTime != null)
                            {
                                outp = (TimeSpan)_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Controller.Clock.CurrentTime;
                            }
                    return outp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
        }

        /// <summary>
        /// get animation current milli second.
        /// </summary>
        public double Current
        {
            get
            {
                double outp = 0;
                try
                {

                    if (_AnimationClock != null)
                        if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase] != null)
                            if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Controller != null && _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Controller.Clock != null && _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Controller.Clock.CurrentTime != null)
                            {
                                TimeSpan timespan = (TimeSpan)_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Controller.Clock.CurrentTime;
                                outp = timespan.TotalMilliseconds;
                            }
                    return outp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        /// <summary>
        /// get CurrentState
        /// </summary>
        public ClockState CurrentState
        {
            get
            {
                ClockState outp = ClockState.Stopped;
                try
                {

                    if (_AnimationClock != null)
                        if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase] != null)
                            if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Controller != null && _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Controller.Clock != null && _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Controller.Clock.CurrentState != null)
                            {
                                outp = (ClockState)_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Controller.Clock.CurrentState;
                            }
                    return outp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
        }

        public double Duration
        {
            get
            {
                double duration = 0;
                try
                {
                    if (_AnimationClock != null && _AnimationClock[(int)_TargetBase] != null)
                        if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase] != null && _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].NaturalDuration != null
                           && _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].NaturalDuration.TimeSpan != null)
                        {
                            duration = _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].NaturalDuration.TimeSpan.TotalMilliseconds;
                        }
                    return duration;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public TimeSpan DurationTime
        {
            get
            {
                TimeSpan duration = new TimeSpan();
                try
                {
                    if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase] != null && _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].NaturalDuration != null
                        && _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].NaturalDuration.TimeSpan != null)
                    {
                        duration = _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].NaturalDuration.TimeSpan;
                    }
                    return duration;
                }
                catch (Exception ex)
                {
                    throw ex;
                }



            }
        }


        public double CurrentProgress
        {
            get
            {
                double progress = 0;
                try
                {
                    if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase] != null && _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].CurrentProgress != null)
                    {
                        progress = (double)_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].CurrentProgress;
                    }
                    return progress;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }


        public AnimationStatus Status
        {
            get
            {
                return _AnimationStatus;
            }
        }


        #endregion

        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="machineModel">machine model</param>
        /// <param name="keyFrames">animation keyframe</param>
        public AnimationCtrl(MachineModel machineModel, AnimationClock[][] animationClock)
        {
            try
            {
                bool searchfound = false;
                _MachineModel = machineModel;
                if (animationClock == null)
                    animationClock = new AnimationClock[6][];

                _AnimationClock = animationClock;

                for (int i = 0; i < _AnimationClock.Length; i++)
                {
                    for (int j = 0; j < _AnimationClock[i].Length; j++)
                    {
                        if (_AnimationClock[i][j] != null)
                        {
                            searchfound = true;
                            _TargetBase = (TargetType)i;
                            _KeyFrameTypeBase = (AnimationKeyFrame)j;
                            break;
                        }
                    }

                    if (searchfound)
                        break;
                }

                if (searchfound)
                {
                    _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Completed -= AnimationCtrl_Completed;
                    _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Completed += new EventHandler(AnimationCtrl_Completed);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Event
        /// <summary>
        /// animation completed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimationCtrl_Completed(object sender, EventArgs e)
        {
            try
            {
                if (AnimationCompleted != null)
                    AnimationCompleted(this.Duration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Public Function
        /// <summary>
        /// set speed animation
        /// </summary>
        /// <param name="speedRatio">speed ratio >0 </param>
        public void SetSpeed(double speedRatio)
        {
            try
            {
                for (int i = 0; i < _AnimationClock.Length; i++)
                {
                    for (int j = 0; j < _AnimationClock[i].Length; j++)
                    {
                        if (_AnimationClock[i][j] != null)
                            _AnimationClock[i][j].Controller.SpeedRatio = speedRatio;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// resume animation 
        /// </summary>
        public void Resume()
        {
            try
            {

                for (int i = 0; i < _AnimationClock.Length; i++)
                {
                    for (int j = 0; j < _AnimationClock[i].Length; j++)
                    {
                        if (_AnimationClock[i][j] != null)
                            _AnimationClock[i][j].Controller.Resume();
                    }
                }
                _IsPause = false;
                _AnimationStatus = AnimationStatus.Resume;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// pause animation
        /// </summary>
        public void Pause()
        {
            try
            {
                for (int i = 0; i < _AnimationClock.Length; i++)
                {
                    for (int j = 0; j < _AnimationClock[i].Length; j++)
                    {
                        if (_AnimationClock[i][j] != null)
                            _AnimationClock[i][j].Controller.Pause();
                    }
                }

                _IsPause = true;
                _AnimationStatus = AnimationStatus.Pause;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// forwardframe animation
        /// </summary>
        /// <param name="fwMillisec">forward time</param>
        public void ForwardFrame(double fwMillisec)
        {
            try
            {
                if (_AnimationClock != null)
                {
                    if (_IsPause)
                    {
                        TimeSpan current = new TimeSpan();
                        if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].CurrentTime != null)
                            current = (TimeSpan)_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].CurrentTime;

                        TimeSpan check = current + TimeSpan.FromMilliseconds(fwMillisec);

                        if (check.TotalMilliseconds >= _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].NaturalDuration.TimeSpan.TotalMilliseconds)
                        {
                            check = _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].NaturalDuration.TimeSpan - TimeSpan.FromMilliseconds(.5);
                        }


                        for (int i = 0; i < _AnimationClock.Length; i++)
                        {
                            for (int j = 0; j < _AnimationClock[i].Length; j++)
                            {
                                if (_AnimationClock[i][j] != null)
                                    _AnimationClock[i][j].Controller.Seek(check, TimeSeekOrigin.BeginTime);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// backward fame animation
        /// </summary>
        /// <param name="bkMillisec">back time</param>
        public void BackwardFrameAnimation(double bkMillisec)
        {
            try
            {
                if (_AnimationClock != null)
                {
                    if (_IsPause)
                    {
                        TimeSpan current = new TimeSpan();
                        if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].CurrentTime != null)
                            current = (TimeSpan)_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].CurrentTime;

                        TimeSpan check = current - TimeSpan.FromMilliseconds(bkMillisec);
                        if (check.TotalMilliseconds < 0)
                            bkMillisec = 0;

                        for (int i = 0; i < _AnimationClock.Length; i++)
                        {
                            for (int j = 0; j < _AnimationClock[i].Length; j++)
                            {
                                if (_AnimationClock[i][j] != null)
                                    _AnimationClock[i][j].Controller.Seek(current - TimeSpan.FromMilliseconds(bkMillisec), TimeSeekOrigin.BeginTime);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// backward fame animation
        /// </summary>
        /// <param name="bkSecond">back time</param>
        public void GotoFrameAnimation(TimeSpan inpTimeSpan)
        {
            try
            {
                if (_AnimationClock != null)
                {

                    if (_AnimationStatus == AnimationStatus.Pause || _AnimationStatus == AnimationStatus.Stop)
                    {

                        for (int i = 0; i < _AnimationClock.Length; i++)
                        {
                            for (int j = 0; j < _AnimationClock[i].Length; j++)
                            {
                                if (_AnimationClock[i][j] != null)
                                {
                                    _AnimationClock[i][j].Controller.Seek(inpTimeSpan, TimeSeekOrigin.BeginTime);
                                    _AnimationClock[i][j].Controller.Pause();
                                }
                            }
                        }

                        _AnimationStatus = AnimationStatus.Pause;

                    }
                    else
                    {
                        for (int i = 0; i < _AnimationClock.Length; i++)
                        {
                            for (int j = 0; j < _AnimationClock[i].Length; j++)
                            {
                                if (_AnimationClock[i][j] != null)
                                    _AnimationClock[i][j].Controller.Seek(inpTimeSpan, TimeSeekOrigin.BeginTime);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// start animation
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                bool checkbegin = false;
                if (_MachineModel != null)
                {
                    for (int i = 0; i < _AnimationClock.Length; i++)
                    {
                        for (int j = 0; j < _AnimationClock[i].Length; j++)
                        {
                            if (_AnimationClock[i][j] != null)
                            {
                                _AnimationClock[i][j].Controller.Begin();
                                checkbegin = true;
                            }
                        }
                    }

                    if (checkbegin)
                    {
                        _IsPause = false;
                        _AnimationStatus = AnimationStatus.Start;
                    }
                }
                return checkbegin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// stop animation 
        /// </summary>    
        /// <returns></returns>
        public bool Stop()
        {
            try
            {

                if (_IsPause)
                {
                    for (int i = 0; i < _AnimationClock.Length; i++)
                    {
                        for (int j = 0; j < _AnimationClock[i].Length; j++)
                        {
                            if (_AnimationClock[i][j] != null)
                            {
                                _AnimationClock[i][j].Controller.Resume();
                                _AnimationClock[i][j].Controller.Stop();
                            }
                        }
                    }
                    _IsPause = false;
                }
                else
                {
                    for (int i = 0; i < _AnimationClock.Length; i++)
                    {
                        for (int j = 0; j < _AnimationClock[i].Length; j++)
                        {
                            if (_AnimationClock[i][j] != null)
                                _AnimationClock[i][j].Controller.Stop();
                        }
                    }
                }
                _AnimationStatus = AnimationStatus.Stop;



                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ClearClock()
        {
            try
            {
                if (_AnimationClock[(int)_TargetBase] != null)
                    if (_AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase] != null)
                        _AnimationClock[(int)_TargetBase][(int)_KeyFrameTypeBase].Completed -= AnimationCtrl_Completed;

                for (int i = 0; i < _AnimationClock.Length; i++)
                {
                    if (_AnimationClock[i] != null)
                    {
                        for (int j = 0; j < _AnimationClock[i].Length; j++)
                        {
                            if (_AnimationClock[i][j] != null && _AnimationClock[i][j].Controller != null)
                            {
                                _AnimationClock[i][j].Controller.Stop();
                                _AnimationClock[i][j].Controller.Remove();
                            }
                            _AnimationClock[i][j] = null;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
