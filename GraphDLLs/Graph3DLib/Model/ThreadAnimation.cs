using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using Graph3DLib.Controller;

namespace Graph3DLib.Model
{
    /// <summary>
    /// Thread Animation Class for Create Animation
    /// </summary>
    public class ThreadAnimation
    {
        #region Private Variable
        /// <summary>
        /// frame per second
        /// </summary>
        private const double _FrameSecond = 0.003;
        /// <summary>
        /// Machine Model
        /// </summary>
        private MachineModel _MachineModel;
        /// <summary>
        /// current key frame
        /// </summary>
        private DoubleAnimationUsingKeyFrames[][] _AnimationKeyFrameData;
        /// <summary>
        /// List of Translate Data
        /// </summary>
        private List<TranslateData>[] _TranslateData;
        /// <summary>
        /// animation clock 
        /// </summary>
        private AnimationClock[][] _AnimationClock;
        /// <summary>
        /// delegate GraphCreatedEventHandler
        /// </summary>        
        public delegate void AnimationCreated(AnimationClock[][] animationClock);
        //public delegate void AnimationCreated(Storyboard storyBoard);
        /// <summary>   
        /// event GraphCreatedEventHandler
        /// </summary>
        public event AnimationCreated OnAnimationCreated = null;
        /// <summary>
        /// Tranform 3D group of all model
        /// </summary>
        private Transform3DGroup[] _Tranform3DGroups;
        /// <summary>
        /// log class
        /// </summary>
        private LogClass _Log4NetClass;
        #endregion

        #region Public Properties
        /// <summary>
        /// Get/Set Machine Model
        /// </summary>
        public MachineModel MachineModel
        {
            get
            {
                return _MachineModel;
            }
            set
            {
                _MachineModel = value;
            }
        }

        /// <summary>
        /// Get/Set TranslateData
        /// </summary>
        public List<TranslateData>[] TranslateData
        {
            get
            {
                return _TranslateData;
            }
            set
            {
                _TranslateData = value;
            }
        }
        /// <summary>
        /// Get Tranform 3D group of all model
        /// </summary>
        public Transform3DGroup[] Tranform3DGroups
        {
            get
            {
                return _Tranform3DGroups;
            }
            set
            {
                _Tranform3DGroups = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ThreadAnimation()
        {

            _Log4NetClass = new LogClass(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            try
            {
                _AnimationKeyFrameData = new DoubleAnimationUsingKeyFrames[6][];
                _AnimationClock = new AnimationClock[6][];

                for (int i = 0; i < 6; i++)
                {
                    _AnimationClock[i] = new AnimationClock[8];
                    _AnimationKeyFrameData[i] = new DoubleAnimationUsingKeyFrames[8];
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ThreadAnimation");
            }

        }
        #endregion

        #region Public Function
        /// <summary>
        /// Create Animation
        /// </summary>
        public void Create()
        {
            try
            {
                CreateAnimationKeyFrame();

                if (_MachineModel != null)
                {
                    if (_AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.OffsetY] != null && _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.OffsetY].KeyFrames.Count > 0)
                    {
                        _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS][(int)AnimationKeyFrame.OffsetY] = _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.OffsetY].Clone();
                    }

                    //Check keyframe for B sensor from Stripper to Ram,if not have frame use frame from above.
                    SetAnimationKeyframeUpper(TargetType.STRIPPER);
                    SetAnimationKeyframeUpper(TargetType.UPPER_PRESS);
                    SetAnimationKeyframeUpper(TargetType.UPPER);

                    //ReCheck keyframe for B sensor from Ram to Stripper,if not have frame use frame from below.
                    SetAnimationKeyframeFromUpperY(TargetType.RAM);
                    SetAnimationKeyframeFromUpperY(TargetType.UPPER);
                    SetAnimationKeyframeFromUpperY(TargetType.UPPER_PRESS);

                    CreateAnimationClock();

                    if (_Tranform3DGroups != null)
                    {
                        CreateAnimation(_Tranform3DGroups, AnimationKeyFrame.OffsetX, TranslateTransform3D.OffsetXProperty);
                        CreateAnimation(_Tranform3DGroups, AnimationKeyFrame.OffsetY, TranslateTransform3D.OffsetYProperty);
                        CreateAnimation(_Tranform3DGroups, AnimationKeyFrame.OffsetZ, TranslateTransform3D.OffsetZProperty);
                        CreateAnimation(_Tranform3DGroups, AnimationKeyFrame.RotationX, AxisAngleRotation3D.AngleProperty);
                        CreateAnimation(_Tranform3DGroups, AnimationKeyFrame.RotationZ, AxisAngleRotation3D.AngleProperty);
                        CreateAnimation(_Tranform3DGroups, AnimationKeyFrame.RotationY, AxisAngleRotation3D.AngleProperty);
                        CreateAnimation(_Tranform3DGroups, AnimationKeyFrame.ScaleX, ScaleTransform3D.ScaleXProperty);
                        CreateAnimation(_Tranform3DGroups, AnimationKeyFrame.ScaleZ, ScaleTransform3D.ScaleZProperty);
                    }

                    for (int i = 0; i < _AnimationKeyFrameData.Length; i++)
                    {
                        for (int j = 0; j < _AnimationKeyFrameData[i].Length; j++)
                        {
                            if (_AnimationClock[i][j] != null)
                            {

                                _AnimationClock[i][j].Controller.Stop();
                            }
                        }
                    }

                    if (OnAnimationCreated != null)
                    {
                        OnAnimationCreated(_AnimationClock);
                    }

                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Create");
            }
        }
        #endregion

        #region Private Function
        /// <summary>
        /// Create Animation Clock
        /// </summary>
        private void CreateAnimationClock()
        {
            try
            {
                for (int i = 0; i < _AnimationKeyFrameData.Length; i++)
                {
                    if (_AnimationClock[i] != null)
                    {
                        for (int j = 0; j < _AnimationKeyFrameData[i].Length; j++)
                        {
                            if (_AnimationKeyFrameData[i][j] != null)
                            {
                                if (_AnimationClock[i][j] != null)
                                {
                                    _AnimationClock[i][j].Controller.Stop();
                                    _AnimationClock[i][j].Controller.Remove();
                                    _AnimationClock[i][j] = null;
                                }

                                if (_AnimationClock[i][j] == null && _AnimationKeyFrameData[i][j].KeyFrames.Count > 0)
                                {
                                    _AnimationClock[i][j] = _AnimationKeyFrameData[i][j].CreateClock();
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreateAnimationClock");
            }
        }

        /// <summary>
        /// create animation key frame
        /// </summary>
        /// <param name="animateKeyFrames"></param>              
        private bool CreateAnimationKeyFrame()
        {
            try
            {
                if (_TranslateData == null)
                    return false;

                if (_TranslateData.Length != 6)
                    return false;

                List<TranslateData>[] inpValue = _TranslateData;

                double timeperpos = 0;

                for (int targetno = 0; targetno < inpValue.Length; targetno++)
                {
                    InitAnimationKeyframeData(targetno);


                    for (int i = 0; i < inpValue[targetno].Count; i++)
                    {
                        timeperpos = _FrameSecond * (i + 1);


                        if (inpValue[targetno][i].OffsetX != null)
                        {
                            _AnimationKeyFrameData[targetno][(int)AnimationKeyFrame.OffsetX].KeyFrames.Add(CreateLinearDoubleKeyFrame(inpValue[targetno][i].OffsetX, timeperpos));
                        }

                        if (inpValue[targetno][i].OffsetY != null)
                        {
                            _AnimationKeyFrameData[targetno][(int)AnimationKeyFrame.OffsetY].KeyFrames.Add(CreateLinearDoubleKeyFrame(inpValue[targetno][i].OffsetY, timeperpos));
                        }

                        if (inpValue[targetno][i].OffsetZ != null)
                        {
                            _AnimationKeyFrameData[targetno][(int)AnimationKeyFrame.OffsetZ].KeyFrames.Add(CreateLinearDoubleKeyFrame(inpValue[targetno][i].OffsetZ, timeperpos));
                        }

                        if (inpValue[targetno][i].RotationX != null)
                        {
                            _AnimationKeyFrameData[targetno][(int)AnimationKeyFrame.RotationX].KeyFrames.Add(CreateLinearDoubleKeyFrame(inpValue[targetno][i].RotationX, timeperpos));
                        }

                        if (inpValue[targetno][i].RotationY != null)
                        {
                            _AnimationKeyFrameData[targetno][(int)AnimationKeyFrame.RotationY].KeyFrames.Add(CreateLinearDoubleKeyFrame(inpValue[targetno][i].RotationY, timeperpos));
                        }

                        if (inpValue[targetno][i].RotationZ != null)
                        {
                            _AnimationKeyFrameData[targetno][(int)AnimationKeyFrame.RotationZ].KeyFrames.Add(CreateLinearDoubleKeyFrame(inpValue[targetno][i].RotationZ, timeperpos));
                        }

                        if (inpValue[targetno][i].ScaleX != null)
                        {
                            _AnimationKeyFrameData[targetno][(int)AnimationKeyFrame.ScaleX].KeyFrames.Add(CreateLinearDoubleKeyFrame(inpValue[targetno][i].ScaleX, timeperpos));
                        }

                        if (inpValue[targetno][i].ScaleZ != null)
                        {
                            _AnimationKeyFrameData[targetno][(int)AnimationKeyFrame.ScaleZ].KeyFrames.Add(CreateLinearDoubleKeyFrame(inpValue[targetno][i].ScaleZ, timeperpos));
                        }
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreateAnimateKeyFrame");
                return false;
            }
        }

        /// <summary>
        /// Set Animation key frame 3 sensor area for Upper Model 
        /// </summary>
        /// <param name="target"></param>
        private void SetAnimationKeyframeUpper(TargetType target)
        {
            try
            {
                DoubleAnimationUsingKeyFrames[] upper_p = _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS];
                DoubleAnimationUsingKeyFrames[] upper = _AnimationKeyFrameData[(int)TargetType.UPPER];
                DoubleAnimationUsingKeyFrames[] ram = _AnimationKeyFrameData[(int)TargetType.RAM];

                if (target == TargetType.STRIPPER)
                {
                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] != null && _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] != null)
                    {
                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX].KeyFrames.Count == 0 && _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ].KeyFrames.Count == 0)
                        {
                            if (upper_p[(int)AnimationKeyFrame.RotationX] != null && upper_p[(int)AnimationKeyFrame.RotationX].KeyFrames.Count > 0)
                            {
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] = _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS][(int)AnimationKeyFrame.RotationX].Clone();
                            }
                            else if (upper[(int)AnimationKeyFrame.RotationX] != null && upper[(int)AnimationKeyFrame.RotationX].KeyFrames.Count > 0)
                            {
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] = _AnimationKeyFrameData[(int)TargetType.UPPER][(int)AnimationKeyFrame.RotationX].Clone();
                            }
                            else if (ram[(int)AnimationKeyFrame.RotationX] != null && ram[(int)AnimationKeyFrame.RotationX].KeyFrames.Count > 0)
                            {
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] = _AnimationKeyFrameData[(int)TargetType.RAM][(int)AnimationKeyFrame.RotationX].Clone();
                            }


                            if (upper_p[(int)AnimationKeyFrame.RotationZ] != null && upper_p[(int)AnimationKeyFrame.RotationZ].KeyFrames.Count > 0)
                            {
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] = _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS][(int)AnimationKeyFrame.RotationZ].Clone();
                            }
                            else if (upper[(int)AnimationKeyFrame.RotationZ] != null && upper[(int)AnimationKeyFrame.RotationZ].KeyFrames.Count > 0)
                            {
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] = _AnimationKeyFrameData[(int)TargetType.UPPER][(int)AnimationKeyFrame.RotationZ].Clone();
                            }
                            else if (ram[(int)AnimationKeyFrame.RotationZ] != null && ram[(int)AnimationKeyFrame.RotationZ].KeyFrames.Count > 0)
                            {
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] = _AnimationKeyFrameData[(int)TargetType.RAM][(int)AnimationKeyFrame.RotationZ].Clone();
                            }
                        }
                    }

                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] != null)
                    {
                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY].KeyFrames.Count == 0)
                        {
                            if (upper_p[(int)AnimationKeyFrame.OffsetY] != null && upper_p[(int)AnimationKeyFrame.OffsetY].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] = _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS][(int)AnimationKeyFrame.OffsetY].Clone();
                            else if (upper[(int)AnimationKeyFrame.OffsetY] != null && upper[(int)AnimationKeyFrame.OffsetY].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] = _AnimationKeyFrameData[(int)TargetType.UPPER][(int)AnimationKeyFrame.OffsetY].Clone();
                            else if (ram[(int)AnimationKeyFrame.OffsetY] != null && ram[(int)AnimationKeyFrame.OffsetY].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] = _AnimationKeyFrameData[(int)TargetType.RAM][(int)AnimationKeyFrame.OffsetY].Clone();
                        }
                    }
                }
                else if (target == TargetType.UPPER_PRESS)
                {
                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] != null && _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] != null)
                    {
                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX].KeyFrames.Count == 0 && _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ].KeyFrames.Count == 0)
                        {
                            if (upper[(int)AnimationKeyFrame.RotationX] != null && upper[(int)AnimationKeyFrame.RotationX].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] = _AnimationKeyFrameData[(int)TargetType.UPPER][(int)AnimationKeyFrame.RotationX].Clone();
                            else if (ram[(int)AnimationKeyFrame.RotationX] != null && ram[(int)AnimationKeyFrame.RotationX].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] = _AnimationKeyFrameData[(int)TargetType.RAM][(int)AnimationKeyFrame.RotationX].Clone();

                            if (upper[(int)AnimationKeyFrame.RotationZ] != null && upper[(int)AnimationKeyFrame.RotationZ].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] = _AnimationKeyFrameData[(int)TargetType.UPPER][(int)AnimationKeyFrame.RotationZ].Clone();
                            else if (ram[(int)AnimationKeyFrame.RotationZ] != null && ram[(int)AnimationKeyFrame.RotationZ].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] = _AnimationKeyFrameData[(int)TargetType.RAM][(int)AnimationKeyFrame.RotationZ].Clone();
                        }
                    }

                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] != null)
                    {
                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY].KeyFrames.Count == 0)
                        {
                            if (upper[(int)AnimationKeyFrame.OffsetY] != null && upper[(int)AnimationKeyFrame.OffsetY].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] = _AnimationKeyFrameData[(int)TargetType.UPPER][(int)AnimationKeyFrame.OffsetY].Clone();
                            else if (ram[(int)AnimationKeyFrame.OffsetY] != null && ram[(int)AnimationKeyFrame.OffsetY].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] = _AnimationKeyFrameData[(int)TargetType.RAM][(int)AnimationKeyFrame.OffsetY].Clone();
                        }
                    }
                }
                else if (target == TargetType.UPPER)
                {
                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] != null && _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] != null)
                    {
                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX].KeyFrames.Count == 0 && _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ].KeyFrames.Count == 0)
                        {
                            if (ram[(int)AnimationKeyFrame.RotationX] != null && ram[(int)AnimationKeyFrame.RotationX].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] = _AnimationKeyFrameData[(int)TargetType.RAM][(int)AnimationKeyFrame.RotationX].Clone();

                            if (ram[(int)AnimationKeyFrame.RotationZ] != null && ram[(int)AnimationKeyFrame.RotationZ].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] = _AnimationKeyFrameData[(int)TargetType.RAM][(int)AnimationKeyFrame.RotationZ].Clone();
                        }
                    }

                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] != null)
                    {
                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY].KeyFrames.Count == 0)
                        {
                            if (ram[(int)AnimationKeyFrame.OffsetY] != null && ram[(int)AnimationKeyFrame.OffsetY].KeyFrames.Count > 0)
                                _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] = _AnimationKeyFrameData[(int)TargetType.RAM][(int)AnimationKeyFrame.OffsetY].Clone();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "SetAnimationKeyframeUpper");
            }
        }

        /// <summary>
        /// Set Animation key Y form upper to below if not have key frame.
        /// </summary>
        /// <param name="target"></param>
        private void SetAnimationKeyframeFromUpperY(TargetType target)
        {
            try
            {
                DoubleAnimationUsingKeyFrames upperkey_y = null;
                DoubleAnimationUsingKeyFrames upperpresskey_y = null;
                DoubleAnimationUsingKeyFrames stripperkey_y = null;

                DoubleAnimationUsingKeyFrames upperkey_x = null;
                DoubleAnimationUsingKeyFrames upperpresskey_x = null;
                DoubleAnimationUsingKeyFrames stripperkey_x = null;

                DoubleAnimationUsingKeyFrames upperkey_z = null;
                DoubleAnimationUsingKeyFrames upperpresskey_z = null;
                DoubleAnimationUsingKeyFrames stripperkey_z = null;

                if (target == TargetType.RAM)
                {
                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] != null)
                    {
                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY].KeyFrames.Count == 0)
                        {
                            upperkey_y = _AnimationKeyFrameData[(int)TargetType.UPPER][(int)AnimationKeyFrame.OffsetY];
                            upperpresskey_y = _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS][(int)AnimationKeyFrame.OffsetY];
                            stripperkey_y = _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.OffsetY];
                        }

                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX].KeyFrames.Count == 0)
                        {
                            upperkey_x = _AnimationKeyFrameData[(int)TargetType.UPPER][(int)AnimationKeyFrame.RotationX];
                            upperpresskey_x = _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS][(int)AnimationKeyFrame.RotationX];
                            stripperkey_x = _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.RotationX];
                        }

                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ].KeyFrames.Count == 0)
                        {
                            upperkey_z = _AnimationKeyFrameData[(int)TargetType.UPPER][(int)AnimationKeyFrame.RotationZ];
                            upperpresskey_z = _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS][(int)AnimationKeyFrame.RotationZ];
                            stripperkey_z = _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.RotationZ];
                        }
                    }
                }
                else if (target == TargetType.UPPER)
                {
                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] != null)
                    {
                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY].KeyFrames.Count == 0)
                        {
                            upperkey_y = null;
                            upperpresskey_y = _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS][(int)AnimationKeyFrame.OffsetY];
                            stripperkey_y = _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.OffsetY];
                        }
                    }

                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX].KeyFrames.Count == 0)
                    {
                        upperkey_x = null;
                        upperpresskey_x = _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS][(int)AnimationKeyFrame.RotationX];
                        stripperkey_x = _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.RotationX];
                    }

                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ].KeyFrames.Count == 0)
                    {
                        upperkey_z = null;
                        upperpresskey_z = _AnimationKeyFrameData[(int)TargetType.UPPER_PRESS][(int)AnimationKeyFrame.RotationZ];
                        stripperkey_z = _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.RotationZ];
                    }
                }
                else if (target == TargetType.UPPER_PRESS)
                {
                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] != null)
                    {
                        if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY].KeyFrames.Count == 0)
                        {
                            upperkey_y = null;
                            upperpresskey_y = null;
                            stripperkey_y = _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.OffsetY];
                        }
                    }

                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX].KeyFrames.Count == 0)
                    {
                        upperkey_x = null;
                        upperpresskey_x = null;
                        stripperkey_x = _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.RotationX];
                    }

                    if (_AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ].KeyFrames.Count == 0)
                    {
                        upperkey_z = null;
                        upperpresskey_z = null;
                        stripperkey_z = _AnimationKeyFrameData[(int)TargetType.STRIPPER][(int)AnimationKeyFrame.RotationZ];
                    }

                }


                if (upperkey_y != null && upperkey_y.KeyFrames.Count > 0)
                    _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] = upperkey_y.Clone();
                else if (upperpresskey_y != null && upperpresskey_y.KeyFrames.Count > 0)
                    _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] = upperpresskey_y.Clone();
                else if (stripperkey_y != null && stripperkey_y.KeyFrames.Count > 0)
                    _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.OffsetY] = stripperkey_y.Clone();

                if (upperkey_x != null && upperkey_x.KeyFrames.Count > 0)
                    _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] = upperkey_x.Clone();
                else if (upperpresskey_x != null && upperpresskey_x.KeyFrames.Count > 0)
                    _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] = upperpresskey_x.Clone();
                else if (stripperkey_x != null && stripperkey_x.KeyFrames.Count > 0)
                    _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationX] = stripperkey_x.Clone();

                if (upperkey_z != null && upperkey_z.KeyFrames.Count > 0)
                    _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] = upperkey_z.Clone();
                else if (upperpresskey_z != null && upperpresskey_z.KeyFrames.Count > 0)
                    _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] = upperpresskey_z.Clone();
                else if (stripperkey_z != null && stripperkey_z.KeyFrames.Count > 0)
                    _AnimationKeyFrameData[(int)target][(int)AnimationKeyFrame.RotationZ] = stripperkey_z.Clone();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "SetAnimationKeyframeFromUpperY");
            }
        }

        /// <summary>
        /// apply animation keyframes
        /// </summary>
        /// <param name="tranform"></param>
        /// <param name="target"></param>
        /// <param name="keyFramesType"></param>
        /// <param name="properties"></param>
        private void ApplyAnimationKeyFrames(Transform3D tranform, TargetType target, AnimationKeyFrame keyFramesType, System.Windows.DependencyProperty properties)
        {
            try
            {
                TranslateTransform3D translate = tranform as TranslateTransform3D;
                RotateTransform3D rotate = tranform as RotateTransform3D;
                ScaleTransform3D scale = tranform as ScaleTransform3D;

                if (translate != null)
                {
                    translate.ApplyAnimationClock(properties, _AnimationClock[(int)target][(int)keyFramesType], HandoffBehavior.SnapshotAndReplace);
                }
                else if (rotate != null)
                {
                    AxisAngleRotation3D axisrotate = rotate.Rotation as AxisAngleRotation3D;
                    if (axisrotate == null)
                        axisrotate = new AxisAngleRotation3D();
                    if (keyFramesType == AnimationKeyFrame.RotationX)
                        axisrotate.Axis = new Vector3D(1, 0, 0);
                    else if (keyFramesType == AnimationKeyFrame.RotationY)
                        axisrotate.Axis = new Vector3D(0, 1, 0);
                    else if (keyFramesType == AnimationKeyFrame.RotationZ)
                        axisrotate.Axis = new Vector3D(0, 0, 1);

                    rotate.Rotation = axisrotate;
                    rotate.CenterX = 0;
                    rotate.CenterY = 8.75;
                    rotate.CenterZ = 0;

                    double center = 0;


                    Model3DGroup upgroup = _MachineModel.UpperPartModel3DGroup.Children[0] as Model3DGroup;
                    center = upgroup.Bounds.Y + (upgroup.Bounds.SizeY / 2);
                    //if (target == TargetType.STRIPPER)
                    //    center = upgroup.Children[3].Bounds.Y + (upgroup.Children[3].Bounds.SizeY / 2);
                    //else if (target == TargetType.UPPER_PRESS)
                    //    center = upgroup.Children[2].Bounds.Y + (upgroup.Children[2].Bounds.SizeY / 2);
                    //else if (target == TargetType.UPPER)
                    //    center = upgroup.Children[1].Bounds.Y + (upgroup.Children[1].Bounds.SizeY / 2);
                    //else if (target == TargetType.RAM)
                    //    center = upgroup.Children[0].Bounds.Y + (upgroup.Children[0].Bounds.SizeY / 2);

                    rotate.CenterY = center;

                    axisrotate.ApplyAnimationClock(properties, _AnimationClock[(int)target][(int)keyFramesType], HandoffBehavior.SnapshotAndReplace);
                }
                else if (scale != null)
                {
                    scale.CenterX = 0;
                    scale.CenterY = 0;
                    scale.CenterZ = 0;
                    double center = 0;
                    Model3DGroup upgroup = _MachineModel.UpperPartModel3DGroup.Children[0] as Model3DGroup;
                    Model3DGroup downgroup = _MachineModel.LowerPartModel3DGroup.Children[0] as Model3DGroup;

                    if (keyFramesType == AnimationKeyFrame.ScaleX)
                    {
                        if (target == TargetType.STRIPPER)
                            center = upgroup.Children[3].Bounds.X;
                        else if (target == TargetType.UPPER_PRESS)
                            center = upgroup.Children[2].Bounds.X;
                        else if (target == TargetType.UPPER)
                            center = upgroup.Children[1].Bounds.X;

                        if (target == TargetType.LOWER)
                            center = downgroup.Children[1].Bounds.X;
                        else if (target == TargetType.LOWER_PRESS)
                            center = downgroup.Children[2].Bounds.X;

                        scale.CenterX = center;
                    }
                    else if (keyFramesType == AnimationKeyFrame.ScaleZ)
                    {
                        if (target == TargetType.STRIPPER)
                            center = upgroup.Children[3].Bounds.Z;
                        else if (target == TargetType.UPPER_PRESS)
                            center = upgroup.Children[2].Bounds.Z;
                        else if (target == TargetType.UPPER)
                            center = upgroup.Children[1].Bounds.Z;

                        if (target == TargetType.LOWER)
                            center = downgroup.Children[1].Bounds.Z;
                        else if (target == TargetType.LOWER_PRESS)
                            center = downgroup.Children[2].Bounds.Z;

                        scale.CenterZ = center;
                    }

                    scale.ApplyAnimationClock(properties, _AnimationClock[(int)target][(int)keyFramesType], HandoffBehavior.SnapshotAndReplace);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ApplyAnimationKeyFrames");
            }
        }

        /// <summary>
        /// create linear double keyframe
        /// </summary>
        /// <param name="dataInput"></param>
        /// <param name="timePerPos"></param>
        /// <returns></returns>
        private LinearDoubleKeyFrame CreateLinearDoubleKeyFrame(double? dataInput, double timePerPos)
        {
            try
            {
                LinearDoubleKeyFrame doublekeyframe = null;
                if (dataInput != null)
                {
                    double data = Convert.ToDouble(dataInput);
                    doublekeyframe = new LinearDoubleKeyFrame(data, TimeSpan.FromSeconds(timePerPos));
                }
                return doublekeyframe;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreateLinearDoubleKeyFrame");
                return null;
            }
        }

        /// <summary>
        /// initial animation keyframe data
        /// </summary>
        /// <param name="targetno"></param>
        private void InitAnimationKeyframeData(int targetno)
        {
            try
            {
                for (int i = 0; i < _AnimationKeyFrameData[targetno].Length; i++)
                {

                    if (_AnimationKeyFrameData[targetno][i] == null)
                    {
                        _AnimationKeyFrameData[targetno][i] = new DoubleAnimationUsingKeyFrames();
                        _AnimationKeyFrameData[targetno][i].KeyFrames = new DoubleKeyFrameCollection();
                    }
                    else
                    {
                        _AnimationKeyFrameData[targetno][i].KeyFrames.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "InitAnimationKeyframeData");
            }

        }
        /// <summary>
        /// Create animation
        /// </summary>
        /// <param name="tranforms"></param>
        /// <param name="keyframe"></param>
        /// <param name="properites"></param>
        private void CreateAnimation(Transform3DGroup[] tranforms, AnimationKeyFrame keyframe, System.Windows.DependencyProperty properites)
        {
            try
            {
                for (int i = 0; i < _AnimationKeyFrameData.Length; i++)
                {
                    if (_AnimationKeyFrameData[i][(int)keyframe] != null)
                    {
                        TargetType target = (TargetType)i;

                        ApplyAnimationKeyFrames(tranforms[i].Children[(int)keyframe], target, keyframe, properites);

                        if (target == TargetType.RAM && keyframe == AnimationKeyFrame.OffsetY && properites == TranslateTransform3D.OffsetYProperty)
                            ApplyAnimationKeyFrames(tranforms[6].Children[0], target, keyframe, properites);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreateAnimation");
            }
        }
        #endregion

    }
}
