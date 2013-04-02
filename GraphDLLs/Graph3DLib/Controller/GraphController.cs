using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graph3DLib.Model;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Graph3DLib.Controller
{
    #region Struct Enum
    /// <summary>
    /// Struct TranslateData
    /// </summary>
    public struct TranslateData
    {
        public double? OffsetX;
        public double? OffsetY;
        public double? OffsetZ;
        public double? RotationX;
        public double? RotationY;
        public double? RotationZ;
        public double? DataX;
        public double? ScaleZ;
        public double? ScaleX;
    }


    public class SensorOpposite
    {
        public List<SensorPosition> SensorList0;
        public List<SensorPosition> SensorList1;
        public double Distance;
    }

    /// <summary>
    /// animate key enum
    /// </summary>
    public enum AnimationKeyFrame : int
    {
        OffsetX = 5,
        OffsetY = 6,
        OffsetZ = 7,
        RotationX = 0,
        RotationY = 1,
        RotationZ = 2,
        ScaleX = 3,
        ScaleZ = 4
    }
    #endregion

    /// <summary>
    /// Class Graph Controller
    /// </summary>
    public class GraphController
    {
        #region Private Variable
        /// <summary>
        /// Default Data high value when less than 0
        /// </summary>
        private const double _DefaultDataHigh = 500;
        /// <summary>
        /// frame per second
        /// </summary>
        private const double _FrameSecond = 0.003;
        /// <summary>
        /// Sensor position Data
        /// </summary>
        private SensorPosition[] _SensorPositions;
        /// <summary>
        /// Sensor Info X
        /// </summary>
        private List<SensorPosition>[] _SensorInfoX;
        /// <summary>
        /// Sensor Info Y
        /// </summary>
        private List<SensorPosition>[] _SensorInfoY;
        /// <summary>
        /// Sensor Info Z
        /// </summary>
        private List<SensorPosition>[] _SensorInfoZ;
        /// <summary>
        /// Graph Raw Data
        /// </summary>
        private List<double[]> _GraphRawData;
        /// <summary>
        /// Graph Lower Limit
        /// </summary>
        private double _LowerLimit;
        /// <summary>
        /// MaxData (2000) - MinData (1200)
        /// </summary>
        private double _DataHeight;
        /// <summary>
        /// Upper Plate Position Y
        /// </summary>
        private double _UpperPlatePosY;
        /// <summary>
        /// Upper Model Position
        /// </summary>
        private Rect3D[] _UpperModelPositions;
        /// <summary>
        /// Max Offset Y
        /// </summary>
        private double _MaxOffsetY;
        /// <summary>
        /// Max Offset X and Z
        /// </summary>
        private double _MaxOffsetXZ;
        /// <summary>
        /// Max Scale X and Z
        /// </summary>
        private double _MaxScaleXZ;
        /// <summary>
        /// current key frame
        /// </summary>
        //private DoubleAnimationUsingKeyFrames[][] _AnimationKeyFrameData;
        private List<TranslateData>[] _TranslateData;
        /// <summary>
        /// machine model class
        /// </summary>
        private MachineModel _MachineModel;
        /// <summary>
        /// machine hight point
        /// </summary>
        private double _HighValue = 0;
        /// <summary>
        /// machine low point
        /// </summary>
        private double _LowValue = 0;
        /// <summary>
        /// Keep over limit of GraphRawData array position
        /// </summary>
        private List<System.Windows.Point> _OverLimitPosition;
        /// <summary>
        /// Ram Channel poistion for meter
        /// </summary>
        private int _RamChannelPosition;
        /// <summary>
        /// Tranform 3D group of all model
        /// </summary>
        private Transform3DGroup[] _Tranform3DGroups;
        /// <summary>
        /// Sensors High Value 0 to 9
        /// </summary>
        private double[] _SensorsHighValue;
        /// <summary>
        /// Sensors Low Value 0 to 9
        /// </summary>
        private double[] _SensorsLowValue;
        /// <summary>
        /// GraphInfo
        /// </summary>
        private GraphInfo _GraphInfo;
        /// <summary>
        /// keep sensors X opposite case (scale)
        /// </summary>
        private SensorOpposite[] _SensorXOppositeArrays = null;
        /// <summary>
        /// keep sensors Z opposite case (scale)
        /// </summary>
        private SensorOpposite[] _SensorZOppositeArrays = null;
        /// <summary>
        /// keep models size/realsize scale.
        /// </summary>
        private double[] _DataScaleforEachObject;
        #endregion

        #region Public Properties
        /// <summary>
        /// Sensors High Value 0 to 9
        /// </summary>
        public double[] SensorsHighValue
        {
            get
            {
                return _SensorsHighValue;
            }
            set
            {
                _SensorsHighValue = value;
            }
        }

        /// <summary>
        /// Sensors Low Value 0 to 9
        /// </summary>
        public double[] SensorsLowValue
        {
            get
            {
                return _SensorsLowValue;
            }
            set
            {
                _SensorsLowValue = value;
            }
        }
        /// <summary>
        /// Get GraphRawData
        /// </summary>
        public List<double[]> GraphRawData
        {
            get
            {
                return _GraphRawData;
            }
        }

        /// <summary>
        /// Get TranslateData
        /// </summary>
        public List<TranslateData>[] TranslateData
        {
            get
            {
                return _TranslateData;
            }
        }

        /// <summary>
        /// Get/Set GraphInfo
        /// </summary>
        public GraphInfo GraphInfo
        {
            set
            {
                _GraphInfo = value;
            }
            get
            {
                return _GraphInfo;
            }
        }

        /// <summary>
        /// Get/Set Sensorposition
        /// </summary>
        public SensorPosition[] Sensorpositions
        {
            get
            {
                return _SensorPositions;
            }
            set
            {
                _SensorPositions = value;
            }
        }

        /// <summary>
        /// Get/Set SensorInfo X
        /// </summary>
        public List<SensorPosition>[] SensorInfoX
        {
            get
            {
                return _SensorInfoX;
            }
            set
            {
                _SensorInfoX = value;
            }
        }
        /// <summary>
        /// Get/Set SensorInfo Y
        /// </summary>
        public List<SensorPosition>[] SensorInfoY
        {
            get
            {
                return _SensorInfoY;
            }
            set
            {
                _SensorInfoY = value;
            }
        }
        /// <summary>
        /// Get/Set SensorInfo Z
        /// </summary>
        public List<SensorPosition>[] SensorInfoZ
        {
            get
            {
                return _SensorInfoZ;
            }
            set
            {
                _SensorInfoZ = value;
            }
        }
        /// <summary>
        /// Data High Value default is 2000
        /// </summary>
        public double DataHighValue
        {
            get
            {
                return _HighValue;
            }
        }
        /// <summary>
        /// Data Low Value defalut is 1200
        /// </summary>
        public double DataLowValue
        {
            get
            {
                return _LowValue;
            }
        }

        /// <summary>
        /// Ram DataPosition
        /// </summary>
        public int RamDataPosition
        {
            get
            {
                return _RamChannelPosition;
            }
        }
        /// <summary>
        /// Get OverLimitPosition X=over pos , Y = length
        /// </summary>
        public List<System.Windows.Point> OverLimitPosition
        {
            get
            {
                return _OverLimitPosition;
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
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="machineModel">machine model</param>
        public GraphController(MachineModel machineModel)
        {
            _MachineModel = machineModel;
            _GraphRawData = new List<double[]>();
            //_HighValue = MachineConfig.ReadDoubleValue("SensorHighValue");
            //_LowValue = MachineConfig.ReadDoubleValue("SensorLowValue");
            _LowerLimit = (_MachineModel.LowerPlatePos.Y + _MachineModel.LowerPlatePos.SizeY);
            _DataHeight = (_HighValue - _LowValue);
            _UpperPlatePosY = _MachineModel.UpperPlatePos.Y;
            _MaxOffsetY = Math.Abs(_UpperPlatePosY - _LowerLimit);
            _MaxOffsetXZ = 15;
            _MaxScaleXZ = 3;
            _OverLimitPosition = new List<System.Windows.Point>();

            _TranslateData = new List<TranslateData>[6];
            _UpperModelPositions = _MachineModel.UpperModelsPosition;
            
            _DataScaleforEachObject = new double[6];

            _Tranform3DGroups = CreateTranform3DGroups();
            for (int i = 0; i < 6; i++)
            {
                _TranslateData[i] = new List<TranslateData>();
            }
        }
        #endregion

        #region Public Function
        /// <summary>
        /// update Sensor info
        /// </summary>
        /// <param name="sensorPos">sensor position</param>
        public void UpdateSensorInfo()
        {
            try
            {
                _SensorInfoX = new List<SensorPosition>[6];
                _SensorInfoY = new List<SensorPosition>[6];
                _SensorInfoZ = new List<SensorPosition>[6];
                _SensorXOppositeArrays = new SensorOpposite[6];
                _SensorZOppositeArrays = new SensorOpposite[6];

                SensorPosition[] sensorpos = _GraphInfo.SensorPositions;
                
                for (int i = 0; i < sensorpos.Length; i++)
                {
                    if (sensorpos[i] != null)
                    {
                        if (sensorpos[i].ChNo != -1 && _SensorsLowValue[i] != 0)
                        {
                            if (sensorpos[i].Way == WayType.INVALID)
                            {
                                if (_SensorInfoY[(int)sensorpos[i].Target] == null)
                                    _SensorInfoY[(int)sensorpos[i].Target] = new List<SensorPosition>();

                                _SensorInfoY[(int)sensorpos[i].Target].Add(sensorpos[i]);
                            }
                            else
                            {
                                if (sensorpos[i].Way == WayType.UP || sensorpos[i].Way == WayType.DOWN)
                                {
                                    if (_SensorInfoZ[(int)sensorpos[i].Target] == null)
                                        _SensorInfoZ[(int)sensorpos[i].Target] = new List<SensorPosition>();

                                    _SensorInfoZ[(int)sensorpos[i].Target].Add(sensorpos[i]);
                                }
                                else if (sensorpos[i].Way == WayType.LEFT || sensorpos[i].Way == WayType.RIGHT)
                                {
                                    if (_SensorInfoX[(int)sensorpos[i].Target] == null)
                                        _SensorInfoX[(int)sensorpos[i].Target] = new List<SensorPosition>();

                                    _SensorInfoX[(int)sensorpos[i].Target].Add(sensorpos[i]);
                                }
                            }

                        }
                    }
                }
                OppositeSensorCalulation(_SensorInfoZ);
                OppositeSensorCalulation(_SensorInfoX);
                _RamChannelPosition = GetRAMChannelPosition();
                UpdateSensorVector();

                Model3DGroup upgroup = _MachineModel.UpperPartModel3DGroup.Children[0] as Model3DGroup;
                Model3DGroup downgroup = _MachineModel.LowerPartModel3DGroup.Children[0] as Model3DGroup;

                if (upgroup != null && downgroup != null)
                {
                    _DataScaleforEachObject[(int)TargetType.STRIPPER] = upgroup.Children[3].Bounds.SizeX / (double)_GraphInfo.MoldPressWidth;
                    _DataScaleforEachObject[(int)TargetType.UPPER_PRESS] = upgroup.Children[2].Bounds.SizeX / (double)_GraphInfo.MoldPressWidth;
                    _DataScaleforEachObject[(int)TargetType.UPPER] = upgroup.Children[1].Bounds.SizeX / (double)_GraphInfo.MoldWidth;
                    _DataScaleforEachObject[(int)TargetType.LOWER] = downgroup.Children[1].Bounds.SizeX / (double)_GraphInfo.MoldWidth;
                    _DataScaleforEachObject[(int)TargetType.LOWER_PRESS] = downgroup.Children[2].Bounds.SizeX / (double)_GraphInfo.MoldPressWidth;
                    _DataScaleforEachObject[(int)TargetType.RAM] = upgroup.Children[0].Bounds.SizeX / (double)_GraphInfo.BolsterWidth;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// read graph data
        /// </summary>
        /// <param name="inpData"></param>
        /// <returns></returns>
        public bool ReadData(List<double[]> inpData)
        {
            try
            {
                bool ret = false;
                bool chkup = false;
                int uppos = 0;
                int downpos = 0;

                for (int i = 0; i < inpData.Count; i++)
                {
                    double[] rawdata = (double[])inpData[i].Clone();

                    if (rawdata[_RamChannelPosition] > _HighValue && !chkup)
                    {
                        chkup = true;
                        uppos = _GraphRawData.Count;
                    }

                    if (rawdata[_RamChannelPosition] < _HighValue && chkup)
                    {
                        chkup = false;
                        downpos = _GraphRawData.Count;

                        System.Windows.Point point = new System.Windows.Point(uppos, downpos);
                        _OverLimitPosition.Add(point);
                    }



                    _GraphRawData.Add(rawdata);
                    CreateTranslateData((double[])rawdata.Clone());
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Clear Graph Data
        /// </summary>
        public void ClearData()
        {
            try
            {
                if (_GraphRawData != null)
                    _GraphRawData.Clear();


                if (_TranslateData != null)
                {
                    for (int j = 0; j < _TranslateData.Length; j++)
                        _TranslateData[j].Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Function
        /// <summary>
        /// Create tranform 3D groups for all model
        /// </summary>
        /// <returns></returns>
        private Transform3DGroup[] CreateTranform3DGroups()
        {
            try
            {
                Transform3DGroup[] tranforms = null;

                if (_MachineModel != null)
                {
                    //_StoryBoard = new Storyboard();
                    Model3DGroup upperpart = _MachineModel.UpperPartModel3DGroup;

                    Model3DGroup upperpartmodelgroup = upperpart.Children[0] as Model3DGroup;
                    Model3DGroup upppershaft = upperpart.Children[1] as Model3DGroup;
                    Model3DGroup upperram = upperpartmodelgroup.Children[0] as Model3DGroup;
                    Model3DGroup upperplate = upperpartmodelgroup.Children[1] as Model3DGroup;
                    Model3DGroup upperpress = upperpartmodelgroup.Children[2] as Model3DGroup;
                    Model3DGroup stripper = upperpartmodelgroup.Children[3] as Model3DGroup;

                    Transform3DGroup upperramtransgroup = upperram.Transform as Transform3DGroup;
                    Transform3DGroup upperplatetransgroup = upperplate.Transform as Transform3DGroup;
                    Transform3DGroup upperpresstransgroup = upperpress.Transform as Transform3DGroup;
                    Transform3DGroup strippertransgroup = stripper.Transform as Transform3DGroup;
                    Transform3DGroup upppershafttransgroup = upppershaft.Transform as Transform3DGroup;


                    Model3DGroup lowerpart = _MachineModel.LowerPartModel3DGroup;

                    Model3DGroup lowerpartmodelgroup = lowerpart.Children[0] as Model3DGroup;
                    Model3DGroup lowerplate = lowerpartmodelgroup.Children[1] as Model3DGroup;
                    Model3DGroup lowerpress = lowerpartmodelgroup.Children[2] as Model3DGroup;

                    Transform3DGroup lowerplatetransgroup = lowerplate.Transform as Transform3DGroup;
                    Transform3DGroup lowerpresstransgroup = lowerpress.Transform as Transform3DGroup;

                    tranforms = new Transform3DGroup[7];
                    tranforms[(int)TargetType.LOWER] = lowerplatetransgroup;
                    tranforms[(int)TargetType.LOWER_PRESS] = lowerpresstransgroup;
                    tranforms[(int)TargetType.RAM] = upperramtransgroup;
                    tranforms[(int)TargetType.STRIPPER] = strippertransgroup;
                    tranforms[(int)TargetType.UPPER] = upperplatetransgroup;
                    tranforms[(int)TargetType.UPPER_PRESS] = upperpresstransgroup;
                    tranforms[6] = upppershafttransgroup;
                }

                return tranforms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Create Vector for 2 sensor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sensor1"></param>
        /// <param name="sensor2"></param>
        /// <param name="baseVector"></param>
        /// <param name="vector90"></param>
        private void CreateVectorTwoSensor(string type, SensorPosition sensor1, SensorPosition sensor2, out Vector3D baseVector, out Vector3D vector90)
        {
            Point3D p1 = new Point3D(sensor1.X, 0, sensor1.Z);
            Point3D p2 = new Point3D(sensor2.X, 0, sensor2.Z);
            baseVector = new Vector3D();
            vector90 = new Vector3D();

            if (type == "Y")
            {
                baseVector = new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
                vector90 = new Vector3D(baseVector.Z, 0, -baseVector.X);
            }
            else if (type == "X")
            {
                baseVector = new Vector3D(p2.X - p1.X, 0, 0);
                vector90 = new Vector3D(baseVector.Y, -baseVector.X, 0);
            }
            else if (type == "Z")
            {
                baseVector = new Vector3D(0, 0, p2.Z - p1.Z);
                vector90 = new Vector3D(baseVector.Y, -baseVector.Z, 0);
            }


        }

        /// <summary>
        /// update sensor vector
        /// </summary>
        private void UpdateSensorVector()
        {
            try
            {
                Vector3D vectorbase = new Vector3D();
                Vector3D vector90 = new Vector3D();
                for (int i = 0; i < _SensorInfoY.Length; i++)
                {
                    if (_SensorInfoY[i] != null)
                    {
                        if (_SensorInfoY[i].Count == 2)
                        {
                            CreateVectorTwoSensor("Y", _SensorInfoY[i][0], _SensorInfoY[i][1], out vectorbase, out vector90);

                            if (_SensorInfoY[i].Count >= 2)
                            {
                                _SensorInfoY[i][0].BaseVector = _SensorInfoY[i][1].BaseVector = vectorbase;
                                _SensorInfoY[i][0].RotationAxis = _SensorInfoY[i][1].RotationAxis = vector90;
                            }

                        }
                    }
                }

                for (int i = 0; i < _SensorInfoX.Length; i++)
                {
                    if (_SensorInfoX[i] != null)
                    {
                        if (_SensorInfoX[i].Count == 2)
                        {
                            CreateVectorTwoSensor("X", _SensorInfoX[i][0], _SensorInfoX[i][1], out vectorbase, out vector90);
                        }
                        else if (_SensorInfoX[i].Count == 3)
                        {
                            CreateVectorTwoSensor("X", _SensorInfoX[i][0], _SensorInfoX[i][2], out vectorbase, out vector90);
                        }

                        if (_SensorInfoX[i].Count >= 2)
                        {
                            _SensorInfoX[i][0].BaseVector = _SensorInfoX[i][1].BaseVector = vectorbase;
                            _SensorInfoX[i][0].RotationAxis = _SensorInfoX[i][1].RotationAxis = vector90;
                        }
                    }
                }

                for (int i = 0; i < _SensorInfoZ.Length; i++)
                {
                    if (_SensorInfoZ[i] != null)
                    {
                        if (_SensorInfoZ[i].Count == 2)
                        {
                            CreateVectorTwoSensor("Z", _SensorInfoZ[i][0], _SensorInfoZ[i][1], out vectorbase, out vector90);
                        }
                        else if (_SensorInfoZ[i].Count == 3)
                        {
                            CreateVectorTwoSensor("Z", _SensorInfoZ[i][0], _SensorInfoZ[i][2], out vectorbase, out vector90);
                        }

                        if (_SensorInfoZ[i].Count >= 2)
                        {
                            _SensorInfoZ[i][0].BaseVector = _SensorInfoZ[i][1].BaseVector = vectorbase;
                            _SensorInfoZ[i][0].RotationAxis = _SensorInfoZ[i][1].RotationAxis = vector90;
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
        /// Create Translate Data
        /// </summary>
        /// <param name="inpData"></param>
        private void CreateTranslateData(double[] inpData)
        {
            try
            {
                TranslateData trandata = new TranslateData();
                trandata.DataX = inpData[0];


                for (int i = 0; i < 6; i++)
                {
                    trandata = CreateTranslateDataY((TargetType)i, inpData);

                    if (_SensorInfoX[i] != null)
                    {
                        int datapos = _SensorInfoX[i][0].ChNo;
                        //Case 1 sensor
                        if (_SensorInfoX[i].Count == 1)
                        {
                            trandata.OffsetX = CalcTranslatePlotX(_SensorInfoX[i][0], inpData[datapos], 1, datapos - 1);
                        }
                        //Case 2 sensor
                        else if (_SensorInfoX[i].Count == 2)
                        {
                            TranslateData trantmp = CalcTwoSensorAxisX(_SensorInfoX[i][0], _SensorInfoX[i][1], inpData);
                            trandata.OffsetX = trantmp.OffsetX;
                            trandata.RotationY = trantmp.RotationY;
                        }
                        //Case 3 sensor
                        else if (_SensorInfoX[i].Count == 3)
                        {
                            TranslateData trantmp = CalcTwoSensorAxisX(_SensorInfoX[i][0], _SensorInfoX[i][2], inpData);
                            trandata.OffsetX = trantmp.OffsetX;
                            trandata.RotationY = trantmp.RotationY;
                        }

                        if (_SensorXOppositeArrays[i] != null)
                        {
                            trandata.ScaleX = CalcOppositeSensorScaleXZ(_SensorXOppositeArrays[i], inpData, "X");
                            if (trandata.ScaleX == 0)
                                trandata.ScaleZ = 0;
                            else
                                trandata.ScaleZ = 1;
                            //trandata.OffsetX = trandata.OffsetX / trandata.ScaleX;
                        }
                    }


                    if (_SensorInfoZ[i] != null)
                    {
                        int datapos = _SensorInfoZ[i][0].ChNo;
                        //Case 1 sensor
                        if (_SensorInfoZ[i].Count == 1)
                        {
                            trandata.OffsetZ = CalcTranslatePlotZ(_SensorInfoZ[i][0], inpData[datapos], 1, datapos - 1);
                        }
                        //Case 2 sensor
                        else if (_SensorInfoZ[i].Count == 2)
                        {
                            TranslateData trantmp = CalcTwoSensorAxisZ(_SensorInfoZ[i][0], _SensorInfoZ[i][1], inpData);
                            trandata.OffsetZ = trantmp.OffsetZ;
                            trandata.RotationY = trantmp.RotationY;
                        }
                        //Case 3 sensor
                        else if (_SensorInfoZ[i].Count == 3)
                        {
                            TranslateData trantmp = CalcTwoSensorAxisZ(_SensorInfoZ[i][0], _SensorInfoZ[i][2], inpData);
                            trandata.OffsetZ = trantmp.OffsetZ;
                            trandata.RotationY = trantmp.RotationY;
                        }

                        if (_SensorZOppositeArrays[i] != null)
                        {
                            trandata.ScaleZ = CalcOppositeSensorScaleXZ(_SensorZOppositeArrays[i], inpData, "Z");
                            //trandata.OffsetZ = trandata.OffsetZ / trandata.ScaleZ;
                            if (trandata.ScaleZ == 0)
                                trandata.ScaleX = 0;
                            else
                                trandata.ScaleX = 1;
                        }
                    }

                    _TranslateData[i].Add(trandata);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create TranslateData Y
        /// </summary>
        /// <param name="target"></param>
        /// <param name="inpData"></param>
        /// <returns></returns>
        private TranslateData CreateTranslateDataY(TargetType target, double[] inpData)
        {
            try
            {
                TranslateData trandata = new TranslateData();
                trandata.DataX = inpData[0];
                bool chkfalse = false;


                if (target == TargetType.STRIPPER)
                {

                    if (_SensorInfoY[(int)target] != null)
                    {
                        //Case 1 sensor
                        if (_SensorInfoY[(int)target].Count == 1)
                        {
                            int datapos = _SensorInfoY[(int)target][0].ChNo;
                            trandata.OffsetY = CalcTranslatePlotY(inpData[datapos], datapos - 1, target);
                        }
                        //Case 2 sensor
                        else if (_SensorInfoY[(int)target].Count == 2)
                        {
                            int datapos1 = _SensorInfoY[(int)target][0].ChNo;
                            int datapos2 = _SensorInfoY[(int)target][1].ChNo;

                            double data = 0;
                            int sensorindex = 0;
                            if (inpData[datapos1] > inpData[datapos2])
                            {
                                sensorindex = _SensorInfoY[(int)target][0].ChNo - 1;
                                data = inpData[datapos1];
                            }
                            else
                            {
                                sensorindex = _SensorInfoY[(int)target][1].ChNo - 1;
                                data = inpData[datapos2];
                            }

                            trandata.OffsetY = CalcTranslatePlotY(data, sensorindex, target);
                            //trandata = CalcTwoSensorAxisY(_SensorInfoY[(int)target][0], _SensorInfoY[(int)target][1], inpData);
                            //trandata.DataX = inpData[0];
                        }
                    }
                }
                else if (target == TargetType.UPPER_PRESS)
                {
                    //Case 1 or 2 sensor
                    if (_SensorInfoY[(int)target] != null)
                        if (_SensorInfoY[(int)target].Count == 1 || _SensorInfoY[(int)target].Count == 2)
                        {
                            List<SensorPosition> listsensor = new List<SensorPosition>();
                            listsensor.AddRange(_SensorInfoY[(int)target]);

                            if (_SensorInfoY[(int)TargetType.STRIPPER] != null)
                                listsensor.AddRange(_SensorInfoY[(int)TargetType.STRIPPER]);

                            //check stripper Y number
                            if (listsensor.Count >= 1)
                            {
                                SensorPosition[] sensors;
                                bool check = CheckThreeSensorArea(listsensor, out sensors);
                                if (check)
                                {
                                    trandata = CalcThreeSensorAxisY(sensors, inpData, target);
                                    trandata.DataX = inpData[0];
                                    chkfalse = true;
                                }
                            }
                        }
                }
                else if (target == TargetType.UPPER)
                {
                    //Case 1 or 2 sensor
                    if (_SensorInfoY[(int)target] != null)
                        if (_SensorInfoY[(int)target].Count == 1 || _SensorInfoY[(int)target].Count == 2)
                        {

                            List<SensorPosition> listsensor = new List<SensorPosition>();
                            listsensor.AddRange(_SensorInfoY[(int)target]);

                            //check UPPER_PRESS Y number 
                            if (_SensorInfoY[(int)TargetType.UPPER_PRESS] != null)
                                listsensor.AddRange(_SensorInfoY[(int)TargetType.UPPER_PRESS]);

                            //check stripper Y number                        
                            if (_SensorInfoY[(int)TargetType.STRIPPER] != null)
                                listsensor.AddRange(_SensorInfoY[(int)TargetType.STRIPPER]);


                            if (listsensor.Count >= 1)
                            {
                                SensorPosition[] sensors;
                                bool check = CheckThreeSensorArea(listsensor, out sensors);
                                if (check)
                                {
                                    trandata = CalcThreeSensorAxisY(sensors, inpData, target);
                                    trandata.DataX = inpData[0];
                                    chkfalse = true;
                                }
                            }
                        }
                }
                else if (target == TargetType.RAM)
                {
                    //Case 1 or 2 sensor
                    if (_SensorInfoY[(int)target] != null)
                        if (_SensorInfoY[(int)target].Count == 1 || _SensorInfoY[(int)target].Count == 2)
                        {

                            List<SensorPosition> listsensor = new List<SensorPosition>();
                            listsensor.AddRange(_SensorInfoY[(int)target]);

                            //check UPPER Y number 
                            if (_SensorInfoY[(int)TargetType.UPPER] != null)
                                listsensor.AddRange(_SensorInfoY[(int)TargetType.UPPER]);

                            //check UPPER_PRESS Y number 
                            if (_SensorInfoY[(int)TargetType.UPPER_PRESS] != null)
                                listsensor.AddRange(_SensorInfoY[(int)TargetType.UPPER_PRESS]);

                            //check stripper Y number                        
                            if (_SensorInfoY[(int)TargetType.STRIPPER] != null)
                                listsensor.AddRange(_SensorInfoY[(int)TargetType.STRIPPER]);

                            //check stripper Y number
                            if (listsensor.Count > 1)
                            {
                                SensorPosition[] sensors;
                                bool check = CheckThreeSensorArea(listsensor, out sensors);
                                if (check)
                                {
                                    trandata = CalcThreeSensorAxisY(sensors, inpData, target);
                                    trandata.DataX = inpData[0];
                                    chkfalse = true;
                                }
                            }
                        }
                }

                if (!chkfalse && target != TargetType.STRIPPER)
                {
                    int datapos1 = 0;
                    int datapos2 = 0;
                    double data = 0;
                    int sensorindex = 0;

                    if (_SensorInfoY[(int)target] != null)
                    {
                        if (_SensorInfoY[(int)target].Count == 1)
                        {
                            datapos1 = _SensorInfoY[(int)target][0].ChNo;
                            data = inpData[datapos1];
                        }
                        else if (_SensorInfoY[(int)target].Count == 2)
                        {
                            datapos1 = _SensorInfoY[(int)target][0].ChNo;
                            datapos2 = _SensorInfoY[(int)target][1].ChNo;


                            if (inpData[datapos1] > inpData[datapos2])
                            {
                                sensorindex = _SensorInfoY[(int)target][0].ChNo - 1;
                                data = inpData[datapos1];
                            }
                            else
                            {
                                sensorindex = _SensorInfoY[(int)target][1].ChNo - 1;
                                data = inpData[datapos2];
                            }
                        }
                        trandata.OffsetY = CalcTranslatePlotY(data, sensorindex, target);
                    }
                }


                //Case 3 sensor
                if (_SensorInfoY[(int)target] != null)
                    if (_SensorInfoY[(int)target].Count >= 3)
                    {
                        SensorPosition[] sensors;
                        bool check = CheckThreeSensorArea(_SensorInfoY[(int)target], out sensors);

                        if (!check)
                        {
                            int datapos = _SensorInfoY[(int)target][0].ChNo;
                            trandata.OffsetY = CalcTranslatePlotY(inpData[datapos], datapos - 1, target);
                        }
                        else
                        {
                            trandata = CalcThreeSensorAxisY(sensors, inpData, target);
                            trandata.DataX = inpData[0];
                        }
                    }


                return trandata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calculate two sensor axis Y
        /// </summary>
        /// <param name="sensor1"></param>
        /// <param name="sensor2"></param>
        /// <param name="inpData"></param>
        /// <returns></returns>
        private TranslateData CalcTwoSensorAxisY(SensorPosition sensor1, SensorPosition sensor2, double[] inpData)
        {
            try
            {
                TranslateData trandata = new TranslateData();

                int datapos1 = sensor1.ChNo;
                int datapos2 = sensor2.ChNo;

                double datavalue1 = DataToRealAxisPlot("Y", inpData[datapos1], WayType.INVALID, sensor1.ChNo - 1, sensor1.Target);
                double datavalue2 = DataToRealAxisPlot("Y", inpData[datapos2], WayType.INVALID, sensor2.ChNo - 1, sensor2.Target);

                Vector3D vectorbase = new Vector3D(sensor1.BaseVector.X, datavalue2 - datavalue1, sensor1.BaseVector.Z);
                Vector3D vectorpos = Vector3D.CrossProduct(vectorbase, sensor1.RotationAxis);

                double anglex = CalcAngleFromAxis("X", vectorpos);
                double anglez = CalcAngleFromAxis("Z", vectorpos);

                trandata.OffsetY = Math.Max(datavalue1, datavalue2);
                trandata.RotationX = anglez - 90;
                trandata.RotationZ = anglex - 90;

                return trandata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calulate 2 sensor axis x
        /// </summary>
        /// <param name="sensor1"></param>
        /// <param name="sensor2"></param>
        /// <param name="inpData"></param>
        /// <returns></returns>
        private TranslateData CalcTwoSensorAxisX(SensorPosition sensor1, SensorPosition sensor2, double[] inpData)
        {
            try
            {
                TranslateData trandata = new TranslateData();

                WayType waytype1 = sensor1.Way;
                WayType waytype2 = sensor2.Way;

                if (waytype1 == WayType.UP || waytype1 == WayType.DOWN || waytype1 == WayType.INVALID)
                {
                    waytype1 = WayType.LEFT;
                }

                if (waytype2 == WayType.UP || waytype2 == WayType.DOWN || waytype2 == WayType.INVALID)
                {
                    waytype2 = WayType.LEFT;
                }


                int datapos1 = sensor1.ChNo;
                int datapos2 = sensor2.ChNo;

                double datavalue1 = DataToRealAxisPlot("X", inpData[datapos1], waytype1, sensor1.ChNo - 1, sensor1.Target);
                double datavalue2 = DataToRealAxisPlot("X", inpData[datapos2], waytype2, sensor2.ChNo - 1, sensor2.Target);


                Vector3D vectorbase = new Vector3D(sensor1.BaseVector.X, sensor1.BaseVector.Y, datavalue2 - datavalue1);
                double anglex = CalcAngleFromAxis("Z", vectorbase);

                if (sensor1.Way == WayType.LEFT)
                    trandata.OffsetX = datavalue1;
                else if (sensor2.Way == WayType.LEFT)
                    trandata.OffsetX = datavalue2;
                else
                    trandata.OffsetX = (datavalue1 + datavalue2) / 2;


                trandata.RotationY = anglex - 90;

                return trandata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calculate 2 sensor axis z
        /// </summary>
        /// <param name="sensor1"></param>
        /// <param name="sensor2"></param>
        /// <param name="inpData"></param>
        /// <returns></returns>
        private TranslateData CalcTwoSensorAxisZ(SensorPosition sensor1, SensorPosition sensor2, double[] inpData)
        {
            try
            {
                TranslateData trandata = new TranslateData();

                WayType waytype1 = sensor1.Way;
                WayType waytype2 = sensor2.Way;

                if (waytype1 == WayType.LEFT || waytype1 == WayType.RIGHT || waytype1 == WayType.INVALID)
                {
                    waytype1 = WayType.UP;
                }

                if (waytype2 == WayType.LEFT || waytype2 == WayType.RIGHT || waytype2 == WayType.INVALID)
                {
                    waytype2 = WayType.UP;
                }


                int datapos1 = sensor1.ChNo;
                int datapos2 = sensor2.ChNo;

                double datavalue1 = DataToRealAxisPlot("Z", inpData[datapos1], waytype1, sensor1.ChNo - 1, sensor1.Target);
                double datavalue2 = DataToRealAxisPlot("Z", inpData[datapos2], waytype2, sensor2.ChNo - 1, sensor2.Target);


                Vector3D vectorbase = new Vector3D(datavalue2 - datavalue1, sensor1.BaseVector.Y, sensor1.BaseVector.Z);

                double anglez = CalcAngleFromAxis("X", vectorbase);

                //trandata.OffsetZ = Math.Max(datavalue1, datavalue2);
                if (sensor1.Way == WayType.LEFT)
                    trandata.OffsetZ = datavalue1;
                else if (sensor2.Way == WayType.LEFT)
                    trandata.OffsetZ = datavalue2;
                else
                    trandata.OffsetZ = (datavalue1 + datavalue2) / 2;
                trandata.RotationY = anglez - 90;

                return trandata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calculate 3 sensor axis Y
        /// </summary>
        /// <param name="sensors"></param>
        /// <param name="inpData"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private TranslateData CalcThreeSensorAxisY(SensorPosition[] sensors, double[] inpData, TargetType target)
        {
            try
            {
                TranslateData trandata = new TranslateData();

                int datapos1 = sensors[0].ChNo;
                int datapos2 = sensors[1].ChNo;
                int datapos3 = sensors[2].ChNo;

                double[] datavalues = new double[3];
                datavalues[0] = DataToRealAxisPlot("Y", inpData[datapos1], WayType.INVALID, sensors[0].ChNo - 1, sensors[0].Target);
                datavalues[1] = DataToRealAxisPlot("Y", inpData[datapos2], WayType.INVALID, sensors[1].ChNo - 1, sensors[1].Target);
                datavalues[2] = DataToRealAxisPlot("Y", inpData[datapos3], WayType.INVALID, sensors[2].ChNo - 1, sensors[2].Target);

                double datascale = _DataScaleforEachObject[(int)target];

                Point3D p1 = new Point3D(sensors[0].X * datascale, datavalues[0], sensors[0].Z * datascale);
                Point3D p2 = new Point3D(sensors[1].X * datascale, datavalues[1], sensors[1].Z * datascale);
                Point3D p3 = new Point3D(sensors[2].X * datascale, datavalues[2], sensors[2].Z * datascale);

                Vector3D v1 = new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
                Vector3D v2 = new Vector3D(p3.X - p1.X, p3.Y - p1.Y, p3.Z - p1.Z);

                Vector3D crosssum = Vector3D.CrossProduct(v1, v2);

                double anglex = CalcAngleFromAxis("X", crosssum);
                double anglez = CalcAngleFromAxis("Z", crosssum);


                //trandata.OffsetY = Math.Max(Math.Max(datavalues[0], datavalues[1]), datavalues[2]);
                trandata.OffsetY = Math.Min(Math.Min(datavalues[0], datavalues[1]), datavalues[2]);
                trandata.RotationX = anglez - 90;
                trandata.RotationZ = anglex - 90;

                return trandata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OppositeSensorCalulation(List<SensorPosition>[] senPosList)
        {
            bool isx = false;

            for (int i = 0; i < senPosList.Length; i++)
            {
                List<SensorPosition> senposleft = new List<SensorPosition>();
                List<SensorPosition> senposright = new List<SensorPosition>();

                if (senPosList[i] != null)
                {
                    if (senPosList[i].Count > 1)
                    {
                        foreach (SensorPosition senpos in senPosList[i])
                        {
                            if (senpos.Way == WayType.LEFT)
                            {
                                senposleft.Add(senpos);
                                isx = true;
                            }
                            else if (senpos.Way == WayType.RIGHT)
                            {
                                senposright.Add(senpos);
                                isx = true;
                            }
                            else if (senpos.Way == WayType.UP)
                            {
                                senposright.Add(senpos);
                                isx = false;
                            }
                            else if (senpos.Way == WayType.DOWN)
                            {
                                senposright.Add(senpos);
                                isx = false;
                            }
                        }
                    }

                    if (senposleft.Count > 0 && senposright.Count > 0)
                    {
                        SensorOpposite[] senopposite = null;
                        if (isx)
                            senopposite = _SensorXOppositeArrays;
                        else
                            senopposite = _SensorZOppositeArrays;

                        if (senopposite[i] == null)
                            senopposite[i] = new SensorOpposite();

                        senopposite[i].SensorList0 = senposleft;
                        senopposite[i].SensorList1 = senposright;

                        //if (isx)
                        //    senopposite[i].Distance = senposright[0].X - senposleft[0].X;
                        //else
                        //    senopposite[i].Distance = senposright[0].Z - senposleft[0].Z;


                        Model3DGroup upgroup = _MachineModel.UpperPartModel3DGroup.Children[0] as Model3DGroup;
                        Model3DGroup downgroup = _MachineModel.LowerPartModel3DGroup.Children[0] as Model3DGroup;

                        TargetType target = senposright[0].Target;
                        Model3D model = null;
                        double objectwidth = 0;
                        if (target == TargetType.STRIPPER)
                            model = upgroup.Children[3];
                        else if (target == TargetType.UPPER_PRESS)
                            model = upgroup.Children[2];
                        else if (target == TargetType.UPPER)
                            model = upgroup.Children[1];
                        else if (target == TargetType.LOWER)
                            model = downgroup.Children[1];
                        else if (target == TargetType.LOWER_PRESS)
                            model = downgroup.Children[2];

                        if (isx)
                        {
                            objectwidth = model.Bounds.SizeX;
                        }
                        else
                        {
                            objectwidth = model.Bounds.SizeZ;
                        }

                        senopposite[i].Distance = objectwidth;
                    }
                }
            }
        }

        /// <summary>
        /// Check 3 sensor are in area
        /// </summary>
        /// <param name="listSensor">input list sensor for check</param>
        /// <param name="sensors">return sensor that are in currect position</param>
        /// <returns>true if in area</returns>
        private bool CheckThreeSensorArea(List<SensorPosition> listSensor, out SensorPosition[] sensors)
        {
            try
            {
                int[] zone = new int[4];
                List<SensorPosition> sensorlist = new List<SensorPosition>();
                //sensors = new SensorPosition[4];

                for (int i = 0; i < listSensor.Count; i++)
                {
                    if (listSensor[i].X > 0 && listSensor[i].Z > 0)
                    {
                        zone[0] = 1;
                        //sensors[0] = listSensor[i];
                        sensorlist.Add(listSensor[i]);
                    }
                    else if (listSensor[i].X < 0 && listSensor[i].Z > 0)
                    {
                        //sensors[1] = listSensor[i];
                        sensorlist.Add(listSensor[i]);
                        zone[1] = 1;
                    }
                    else if (listSensor[i].X < 0 && listSensor[i].Z < 0)
                    {
                        //sensors[2] = listSensor[i];
                        sensorlist.Add(listSensor[i]);
                        zone[2] = 1;
                    }
                    else if (listSensor[i].X > 0 && listSensor[i].Z < 0)
                    {
                        //sensors[3] = listSensor[i];
                        sensorlist.Add(listSensor[i]);
                        zone[3] = 1;
                    }
                }

                if (zone[0] + zone[1] + zone[2] + zone[3] >= 3)
                {
                    sensors = sensorlist.ToArray();
                    return true;
                }
                else
                {
                    sensors = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calculate translate ploy Y
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        private double CalcTranslatePlotY(double inputData, int sensorIndex, TargetType targetType)
        {
            double outputdata = 0;

            outputdata = DataToRealAxisPlot("Y", inputData, WayType.INVALID, sensorIndex, targetType);

            return outputdata;
        }

        /// <summary>
        /// Calculate Translate plot X
        /// </summary>
        /// <param name="senserPos"></param>
        /// <param name="inputData"></param>
        /// <param name="sensorNo"></param>
        /// <returns></returns>
        private double CalcTranslatePlotX(SensorPosition senserPos, double inputData, int sensorNo, int sensorIndex)
        {

            WayType waytype = senserPos.Way;

            if (waytype == WayType.UP || waytype == WayType.DOWN || waytype == WayType.INVALID)
            {
                waytype = WayType.LEFT;
            }

            double outputdata = 0;
            if (sensorNo == 1)
            {
                outputdata = DataToRealAxisPlot("X", inputData, waytype, sensorIndex, senserPos.Target);
            }
            if (sensorNo == 2)
            {

            }
            return outputdata;
        }

        /// <summary>
        /// Calculate Translate Plot Z
        /// </summary>
        /// <param name="senserPos"></param>
        /// <param name="inputData"></param>
        /// <param name="sensorNo"></param>
        /// <returns></returns>
        private double CalcTranslatePlotZ(SensorPosition senserPos, double inputData, int sensorNo, int sensorIndex)
        {
            WayType waytype = senserPos.Way;

            if (waytype == WayType.LEFT || waytype == WayType.RIGHT || waytype == WayType.INVALID)
            {
                waytype = WayType.UP;
            }

            double outputdata = 0;
            if (sensorNo == 1)
            {
                outputdata = DataToRealAxisPlot("Z", inputData, waytype, sensorIndex, senserPos.Target);
            }
            return outputdata;
        }

        private double? CalcOppositeSensorScaleXZ(SensorOpposite senserOpposite, double[] inputData, string axisName)
        {
            double? scalex = null;
            if (senserOpposite != null)
            {
                if (senserOpposite.SensorList0.Count == 1 && senserOpposite.SensorList1.Count == 1)
                {
                    int index0 = senserOpposite.SensorList0[0].ChNo - 1;
                    int index1 = senserOpposite.SensorList1[0].ChNo - 1;
                    double data0 = inputData[index0 + 1];
                    double data1 = inputData[index1 + 1];
                    double datahigh0 = _SensorsHighValue[index0] - _SensorsLowValue[index0];
                    double datahigh1 = _SensorsHighValue[index1] - _SensorsLowValue[index1];

                    if (datahigh0 <= 0)
                        datahigh0 = _DefaultDataHigh;

                    if (datahigh1 <= 0)
                        datahigh1 = _DefaultDataHigh;


                    //double x0 = data0 - _SensorsLowValue[index0];
                    //double x1 = data1 - _SensorsLowValue[index1];

                    double x0 = (_MaxOffsetY / datahigh0) * (data0 - _SensorsLowValue[index0]);
                    double x1 = (_MaxOffsetY / datahigh1) * (data1 - _SensorsLowValue[index1]);

                    if (axisName == "X")
                    {
                        if (senserOpposite.SensorList0[0].Way == WayType.RIGHT)
                            x0 *= -1;

                        if (senserOpposite.SensorList1[0].Way == WayType.RIGHT)
                            x1 *= -1;
                    }
                    else if (axisName == "Z")
                    {
                        if (senserOpposite.SensorList0[0].Way == WayType.DOWN)
                            x0 *= -1;

                        if (senserOpposite.SensorList1[0].Way == WayType.DOWN)
                            x1 *= -1;
                    }


                    //double distance = senserOpposite.Distance + ((x1 - x0) / 1000);
                    double distance = senserOpposite.Distance + (x1 - x0);
                    if (distance < 0)
                        distance = 0;

                    scalex = Math.Abs(distance / senserOpposite.Distance);

                    if (scalex > _MaxScaleXZ)
                        scalex = _MaxScaleXZ;
                }
            }
            return scalex;
        }



        /// <summary>
        /// Get Ram Channel Position
        /// </summary>
        /// <returns></returns>
        private int GetRAMChannelPosition()
        {
            int chno = 0;
            if (this.SensorInfoY != null)
            {
                if (this.SensorInfoY[(int)TargetType.RAM] != null)
                {
                    if (this.SensorInfoY[(int)TargetType.RAM].Count > 0)
                        chno = this.SensorInfoY[(int)TargetType.RAM][0].ChNo;
                }
            }

            return chno;
        }

        /// <summary>
        /// Calculate angle from axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="vectorInput"></param>
        /// <returns></returns>
        private double CalcAngleFromAxis(string axis, Vector3D vectorInput)
        {
            Vector3D vectoraxis = new Vector3D();

            if (axis == "X")
                vectoraxis = new Vector3D(1, 0, 0);
            else if (axis == "Y")
                vectoraxis = new Vector3D(0, 1, 0);
            else if (axis == "Z")
                vectoraxis = new Vector3D(0, 0, 1);

            return Math.Round(Vector3D.AngleBetween(vectorInput, vectoraxis), 6);
        }



        /// <summary>
        /// convert csv data to real data plot
        /// </summary>
        /// <param name="inputY"></param>
        /// <returns></returns>
        private double DataToRealAxisPlot(string plotType, double inputY, WayType wayType, int senserIndex, TargetType targetType)
        {
            try
            {
                double outp = 0;
                double datahigh = _SensorsHighValue[senserIndex] - _SensorsLowValue[senserIndex];
                //double midpoint = (_SensorsHighValue[senserIndex] + _SensorsLowValue[senserIndex]) / 2;                
                //double datascale = _DataScaleforEachObject[(int)targetType];

                if (datahigh <= 0)
                    datahigh = _DefaultDataHigh;

                if (plotType == "Y")
                {
                    //outp = ((_MaxOffsetY / _DataHeight) * (inputY - _LowValue)) + (-_MaxOffsetY);                    
                    outp = ((_MaxOffsetY / datahigh) * (inputY - _SensorsLowValue[senserIndex])) + (-_MaxOffsetY);
                    //outp = (datascale * (inputY - _SensorsLowValue[senserIndex]) / 1000) + (-_MaxOffsetY);

                    //if (outp >= 0)
                    //{
                    //    outp = 0;
                    //}
                    //else if (outp <= -_MaxOffsetY - _UpperModelPositions[3].SizeY)
                    //{
                    //    outp = -_MaxOffsetY - _UpperModelPositions[3].SizeY;
                    //}
                }
                else if (plotType == "X" || plotType == "Z")
                {
                    //outp = ((_MaxOffsetXZ / datahigh) * (inputY - midpoint));
                    outp = (_MaxOffsetY / datahigh) * (inputY - _SensorsLowValue[senserIndex]);
                    //outp = (datascale * (inputY - _SensorsLowValue[senserIndex]) / 1000);
                    //outp = datascale * (inputY - _SensorsLowValue[senserIndex]);

                    if (plotType == "X")
                    {
                        if (wayType == WayType.RIGHT)
                        {
                            outp *= -1;
                        }

                        //if (outp <= -_MaxOffsetXZ)
                        //{
                        //    outp = -_MaxOffsetXZ;
                        //}
                        //else if (outp >= _MaxOffsetXZ)
                        //{
                        //    outp = _MaxOffsetXZ;
                        //}

                    }
                    else if (plotType == "Z")
                    {
                        if (wayType == WayType.DOWN)
                            outp *= -1;

                        //if (outp >= _MaxOffsetXZ)
                        //{
                        //    outp = _MaxOffsetXZ;
                        //}
                        //else if (outp <= -_MaxOffsetXZ)
                        //{
                        //    outp = -_MaxOffsetXZ;
                        //}
                    }
                }
                return outp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
