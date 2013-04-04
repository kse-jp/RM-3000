using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows;


namespace Graph3DLib.Model
{
    /// <summary>
    /// enum machine part
    /// </summary>
    public enum MachinePart
    {
        Shaft = 0,
        Stripper = 1,
        UpperPress = 2,
        LowerPress = 3,
        MachinePlate = 4,
        MachineTopBottom = 5,
        MachinePost = 6,
        MachinePostLine = 7,
        Ram = 8
    }

    /// <summary>
    /// machine model class
    /// </summary>
    public class MachineModel
    {
        #region Private Const Variable
        /// <summary>
        /// maximum rotation in Axis X in degree
        /// </summary>
        private const double _MaxRotateXDegree = 80;
        /// <summary>
        /// set top part position for set high between plate
        /// </summary>
        private const double _UpDownToppartPosition = -4.5;
        #endregion

        #region Private Variable
        /// <summary>
        /// current model
        /// </summary>
        private ModelVisual3D _CurrentModel;
        /// <summary>
        /// top plate base position
        /// </summary>
        private Rect3D _TopPlateBasePos;
        /// <summary>
        /// colors for machine part
        /// </summary>
        private Color[] _PartColors;
        /// <summary>
        /// log class
        /// </summary>
        private LogClass _Log4NetClass;

        /// <summary>
        /// Model use image texture
        /// </summary>
        private bool _IsImageTexture;
        /// <summary>
        /// image path from config file
        /// </summary>
        private string _ImagePath;


        #endregion

        #region Public Properties

        /// <summary>
        /// get/set current model
        /// </summary>
        public ModelVisual3D CurrentModel
        {
            get
            {
                return _CurrentModel;
            }
            set
            {
                _CurrentModel = value;
            }
        }

        public bool IsImageTexture
        {
            get
            {
                return _IsImageTexture;
            }
            set
            {
                _IsImageTexture = value;
            }
        }

        public double UpDownTopPartPostion
        {
            get
            {
                return _UpDownToppartPosition;
            }
        }

        /// <summary>
        /// get upperpart Tranform3dgroup
        /// </summary>
        public Transform3DGroup UpperPartTranform3DGroup
        {
            get
            {
                try
                {
                    if (_CurrentModel != null)
                    {
                        Transform3DGroup group = ((Model3DGroup)_CurrentModel.Content).Children[0].Transform as Transform3DGroup;
                        return group;
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// get upperpart model3dgroup
        /// </summary>
        public Model3DGroup UpperPartModel3DGroup
        {
            get
            {
                try
                {
                    if (_CurrentModel != null)
                    {
                        Model3DGroup model = ((Model3DGroup)_CurrentModel.Content).Children[0] as Model3DGroup;
                        return model;
                    }
                    else
                        return null;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// get upperpart model3dgroup
        /// </summary>
        public Model3DGroup LowerPartModel3DGroup
        {
            get
            {
                try
                {
                    if (_CurrentModel != null)
                    {
                        Model3DGroup model = ((Model3DGroup)_CurrentModel.Content).Children[1] as Model3DGroup;
                        return model;
                    }
                    else
                        return null;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// get lowerplate base pos
        /// </summary>
        public Rect3D LowerPlatePos
        {
            get
            {
                try
                {
                    if (_CurrentModel != null)
                    {
                        Model3DGroup bottompart = (Model3DGroup)((Model3DGroup)_CurrentModel.Content).Children[1];
                        Model3DGroup bottomplate = (Model3DGroup)bottompart.Children[0];
                        return bottomplate.Bounds;
                    }
                    else
                        return new Rect3D();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// get upperpart matrix3d
        /// </summary>
        public Matrix3D UpperPartMatrix3D
        {
            get
            {
                try
                {
                    if (_CurrentModel != null)
                    {
                        Transform3DGroup group = ((Model3DGroup)_CurrentModel.Content).Children[0].Transform as Transform3DGroup;

                        return group.Value;
                    }
                    else
                        return new Matrix3D();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Rect3D[] UpperModelsPosition
        {
            get
            {
                try
                {
                    Rect3D[] pos = new Rect3D[4];

                    if (_CurrentModel != null)
                    {

                        Model3DGroup toppart = (Model3DGroup)((Model3DGroup)_CurrentModel.Content).Children[0];
                        Model3DGroup topplate = (Model3DGroup)toppart.Children[0];
                        for (int i = 0; i < pos.Length; i++)
                        {
                            pos[i] = topplate.Children[i].Bounds;
                        }

                        return pos;
                    }
                    else
                        return new Rect3D[4];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// get upperplate position
        /// </summary>
        public Rect3D UpperPlatePos
        {
            get
            {
                try
                {
                    if (_CurrentModel != null)
                    {
                        Model3DGroup toppart = (Model3DGroup)((Model3DGroup)_CurrentModel.Content).Children[0];
                        Model3DGroup topplate = (Model3DGroup)toppart.Children[0];
                        return topplate.Bounds;
                    }
                    else
                        return new Rect3D();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// consturctor
        /// </summary>
        public MachineModel()
        {
            string[] colorparts = MachineConfig.ReadColorStrings();
            _PartColors = ReadColorStrings(colorparts);
            _Log4NetClass = new LogClass(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _IsImageTexture = Convert.ToBoolean(MachineConfig.ReadStringValue("IsImageTexture"));
            _ImagePath = MachineConfig.ReadStringValue("ImagePath");
        }
        #endregion

        #region Private Function

        /// <summary>
        /// read color from string
        /// </summary>
        /// <param name="partColors">string array colors part</param>
        /// <returns>color[]</returns>
        private Color[] ReadColorStrings(string[] partColors)
        {
            try
            {
                Color[] outpcolor = new Color[9];
                for (int i = 0; i < partColors.Length; i++)
                {
                    outpcolor[i] = (Color)ColorConverter.ConvertFromString(partColors[i]);
                }

                return outpcolor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create top part model
        /// </summary>
        /// <returns></returns>
        private Model3DGroup CreateTopPart()
        {

            try
            {
                SquareModel squaremodel = new SquareModel();
                EllipseModel ellipsemodel = new EllipseModel();
                Model3DGroup toppartgroup = new Model3DGroup();
                Model3DGroup topplatemodel = new Model3DGroup();
                Model3DGroup topshaftgroup = new Model3DGroup();

                Transform3DGroup topplatetran = new Transform3DGroup();
                Transform3DGroup topparttran = new Transform3DGroup();

                Point3D[] topmechpoint = new Point3D[8];
                Point3D[] topplatepoint = new Point3D[8];
                Point3D[] toprampoint = new Point3D[8];
                Point3D[] topstripperpoint = new Point3D[8];

                toprampoint[0] = new Point3D(-7, 8 + _UpDownToppartPosition, -3.5);
                toprampoint[1] = new Point3D(7, 8 + _UpDownToppartPosition, -3.5);
                toprampoint[2] = new Point3D(7, 8 + _UpDownToppartPosition, 3.5);
                toprampoint[3] = new Point3D(-7, 8 + _UpDownToppartPosition, 3.5);
                toprampoint[4] = new Point3D(-7, 9.5 + _UpDownToppartPosition, -3.5);
                toprampoint[5] = new Point3D(7, 9.5 + _UpDownToppartPosition, -3.5);
                toprampoint[6] = new Point3D(7, 9.5 + _UpDownToppartPosition, 3.5);
                toprampoint[7] = new Point3D(-7, 9.5 + _UpDownToppartPosition, 3.5);

                topmechpoint[0] = new Point3D(-6, 7 + _UpDownToppartPosition, -3);
                topmechpoint[1] = new Point3D(6, 7 + _UpDownToppartPosition, -3);
                topmechpoint[2] = new Point3D(6, 7 + _UpDownToppartPosition, 3);
                topmechpoint[3] = new Point3D(-6, 7 + _UpDownToppartPosition, 3);
                topmechpoint[4] = new Point3D(-6, 8 + _UpDownToppartPosition, -3);
                topmechpoint[5] = new Point3D(6, 8 + _UpDownToppartPosition, -3);
                topmechpoint[6] = new Point3D(6, 8 + _UpDownToppartPosition, 3);
                topmechpoint[7] = new Point3D(-6, 8 + _UpDownToppartPosition, 3);

                topplatepoint[0] = new Point3D(-5.5, 6 + _UpDownToppartPosition, -2.3);
                topplatepoint[1] = new Point3D(5.5, 6 + _UpDownToppartPosition, -2.3);
                topplatepoint[2] = new Point3D(5.5, 6 + _UpDownToppartPosition, 2.3);
                topplatepoint[3] = new Point3D(-5.5, 6 + _UpDownToppartPosition, 2.3);
                topplatepoint[4] = new Point3D(-5.5, 7 + _UpDownToppartPosition, -2.3);
                topplatepoint[5] = new Point3D(5.5, 7 + _UpDownToppartPosition, -2.3);
                topplatepoint[6] = new Point3D(5.5, 7 + _UpDownToppartPosition, 2.3);
                topplatepoint[7] = new Point3D(-5.5, 7 + _UpDownToppartPosition, 2.3);

                topstripperpoint[0] = new Point3D(-5.5, 5.5 + _UpDownToppartPosition, -2.3);
                topstripperpoint[1] = new Point3D(5.5, 5.5 + _UpDownToppartPosition, -2.3);
                topstripperpoint[2] = new Point3D(5.5, 5.5 + _UpDownToppartPosition, 2.3);
                topstripperpoint[3] = new Point3D(-5.5, 5.5 + _UpDownToppartPosition, 2.3);
                topstripperpoint[4] = new Point3D(-5.5, 6 + _UpDownToppartPosition, -2.3);
                topstripperpoint[5] = new Point3D(5.5, 6 + _UpDownToppartPosition, -2.3);
                topstripperpoint[6] = new Point3D(5.5, 6 + _UpDownToppartPosition, 2.3);
                topstripperpoint[7] = new Point3D(-5.5, 6 + _UpDownToppartPosition, 2.3);

                topplatemodel.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.Ram], toprampoint));
                topplatemodel.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.MachinePlate], topmechpoint));
                topplatemodel.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.UpperPress], topplatepoint));
                topplatemodel.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.Stripper], topstripperpoint));


                topplatemodel.Children[0].Transform = CreateTranform3DGroup();
                topplatemodel.Children[1].Transform = CreateTranform3DGroup();
                topplatemodel.Children[2].Transform = CreateTranform3DGroup();
                topplatemodel.Children[3].Transform = CreateTranform3DGroup();



                topplatetran.Children.Add(new ScaleTransform3D());

                topplatemodel.Transform = topplatetran;

                //if (!_IsImageTexture)
                //{
                topshaftgroup.Children.Add(ellipsemodel.Create(_PartColors[(int)MachinePart.Shaft], new Point3D(0, 8.5 + _UpDownToppartPosition, 0), 5));
                //}
                //else
                //{
                //    topshaftgroup.Children.Add(ellipsemodel.Create(_PartColors[(int)MachinePart.Shaft], @".\\Images\\Shaft.jpg", new Point3D(0, 9.5 + _UpDownToppartPosition, 0), 10));
                //}
                topshaftgroup.Transform = new Transform3DGroup();

                toppartgroup.Children.Add(topplatemodel);
                toppartgroup.Children.Add(topshaftgroup);

                topparttran.Children.Add(new TranslateTransform3D());
                toppartgroup.Children[1].Transform = topparttran;
                //toppartgroup.Transform = topparttran;


                _Log4NetClass.ShowInfo("Create Top Part Model3D Success", "CreateTopPart");
                return toppartgroup;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreateTopPart");
                throw ex;
            }

        }

        private Transform3DGroup CreateTranform3DGroup()
        {
            Transform3DGroup tranform = new Transform3DGroup();
            tranform.Children.Add(new RotateTransform3D());    //X
            tranform.Children.Add(new RotateTransform3D());    //Y
            tranform.Children.Add(new RotateTransform3D());    //Z
            tranform.Children.Add(new ScaleTransform3D());    //X
            tranform.Children.Add(new ScaleTransform3D());    //Z
            tranform.Children.Add(new TranslateTransform3D()); // X
            tranform.Children.Add(new TranslateTransform3D()); // Y
            tranform.Children.Add(new TranslateTransform3D()); // Z


            return tranform;
        }

        /// <summary>
        /// create bottom part model
        /// </summary>
        /// <returns></returns>
        private Model3DGroup CreateBottomPart()
        {
            try
            {
                double rampos_y = -4;
                double mechpos_y = -2.5;
                double platepos_y = -1.5;

                SquareModel squaremodel = new SquareModel();
                Model3DGroup bottompartgroup = new Model3DGroup();
                Model3DGroup bottomplatemodel = new Model3DGroup();
                Model3DGroup bottomshaftgroup = new Model3DGroup();

                Point3D[] bottommechpoint = new Point3D[8];
                Point3D[] bottomplatepoint = new Point3D[8];
                Point3D[] bottomrampoint = new Point3D[8];

                Point3D[] labelfront = new Point3D[4];
                Point3D[] labelback = new Point3D[4];
                Point3D[] labelleft = new Point3D[4];
                Point3D[] labelright = new Point3D[4];

                bottomrampoint[0] = new Point3D(-7, rampos_y, -3.5);
                bottomrampoint[1] = new Point3D(7, rampos_y, -3.5);
                bottomrampoint[2] = new Point3D(7, rampos_y, 3.5);
                bottomrampoint[3] = new Point3D(-7, rampos_y, 3.5);
                bottomrampoint[4] = new Point3D(-7, rampos_y + 1.5, -3.5);
                bottomrampoint[5] = new Point3D(7, rampos_y + 1.5, -3.5);
                bottomrampoint[6] = new Point3D(7, rampos_y + 1.5, 3.5);
                bottomrampoint[7] = new Point3D(-7, rampos_y + 1.5, 3.5);

                bottommechpoint[0] = new Point3D(-6, mechpos_y, -3);
                bottommechpoint[1] = new Point3D(6, mechpos_y, -3);
                bottommechpoint[2] = new Point3D(6, mechpos_y, 3);
                bottommechpoint[3] = new Point3D(-6, mechpos_y, 3);
                bottommechpoint[4] = new Point3D(-6, mechpos_y + 1, -3);
                bottommechpoint[5] = new Point3D(6, mechpos_y + 1, -3);
                bottommechpoint[6] = new Point3D(6, mechpos_y + 1, 3);
                bottommechpoint[7] = new Point3D(-6, mechpos_y + 1, 3);

                bottomplatepoint[0] = new Point3D(-5.5, platepos_y, -2.3);
                bottomplatepoint[1] = new Point3D(5.5, platepos_y, -2.3);
                bottomplatepoint[2] = new Point3D(5.5, platepos_y, 2.3);
                bottomplatepoint[3] = new Point3D(-5.5, platepos_y, 2.3);
                bottomplatepoint[4] = new Point3D(-5.5, platepos_y + 1, -2.3);
                bottomplatepoint[5] = new Point3D(5.5, platepos_y + 1, -2.3);
                bottomplatepoint[6] = new Point3D(5.5, platepos_y + 1, 2.3);
                bottomplatepoint[7] = new Point3D(-5.5, platepos_y + 1, 2.3);

                labelfront[0] = new Point3D(-0.5, rampos_y + 1, 3.5001);
                labelfront[1] = new Point3D(0.5, rampos_y + 1, 3.5001);
                labelfront[2] = new Point3D(0.5, rampos_y, 3.5001);
                labelfront[3] = new Point3D(-0.5, rampos_y, 3.5001);

                labelback[1] = new Point3D(-0.5, 1 + rampos_y, -3.5001);
                labelback[0] = new Point3D(0.5, 1 + rampos_y, -3.5001);
                labelback[3] = new Point3D(0.5, rampos_y, -3.5001);
                labelback[2] = new Point3D(-0.5, rampos_y, -3.5001);

                labelleft[0] = new Point3D(-7.0001, 1 + rampos_y, -0.5);
                labelleft[1] = new Point3D(-7.0001, 1 + rampos_y, 0.5);
                labelleft[2] = new Point3D(-7.0001, rampos_y, 0.5);
                labelleft[3] = new Point3D(-7.0001, rampos_y, -0.5);

                labelright[1] = new Point3D(7.0001, 1 + rampos_y, -0.5);
                labelright[0] = new Point3D(7.0001, 1 + rampos_y, 0.5);
                labelright[3] = new Point3D(7.0001, rampos_y, 0.5);
                labelright[2] = new Point3D(7.0001, rampos_y, -0.5);


                //if (!_IsImageTexture)
                //if (true)
                //{
                bottomplatemodel.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.Ram], bottomrampoint));
                bottomplatemodel.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.MachinePlate], bottommechpoint));
                bottomplatemodel.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.LowerPress], bottomplatepoint));
                //}
                //else
                //{
                //    bottomplatemodel.Children.Add(squaremodel.Create(@".\\Images\\BottomRam.jpg", bottomrampoint));
                //    bottomplatemodel.Children.Add(squaremodel.Create(@".\\Images\\BottomPlate.jpg", bottommechpoint));
                //    bottomplatemodel.Children.Add(squaremodel.Create(@".\\Images\\BottomPress.jpg", bottomplatepoint));
                //}

                bottomplatemodel.Children.Add(squaremodel.CreatePictureLabel(_ImagePath + "F.gif", labelfront));
                bottomplatemodel.Children.Add(squaremodel.CreatePictureLabel(_ImagePath + "B.gif", labelback));
                bottomplatemodel.Children.Add(squaremodel.CreatePictureLabel(_ImagePath + "L.gif", labelleft));
                bottomplatemodel.Children.Add(squaremodel.CreatePictureLabel(_ImagePath + "R.gif", labelright));


                bottomplatemodel.Children[1].Transform = new Transform3DGroup();
                bottomplatemodel.Transform = new Transform3DGroup();

                bottomplatemodel.Children[1].Transform = CreateTranform3DGroup();
                bottomplatemodel.Children[2].Transform = CreateTranform3DGroup();

                bottompartgroup.Children.Add(bottomplatemodel);


                _Log4NetClass.ShowInfo("Create Bottom part Model3D Success", "CreateBottomPart");
                return bottompartgroup;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreateBottomPart");
                throw ex;
            }

        }

        /// <summary>
        /// machine top bottom part
        /// </summary>
        /// <returns></returns>
        private Model3DGroup CreateMachineTopBottomPart()
        {
            try
            {
                SquareModel squaremodel = new SquareModel();
                Model3DGroup machinegroup = new Model3DGroup();

                Point3D[] machinetoppoint = new Point3D[8];
                Point3D[] machinebottompoint = new Point3D[8];



                double width = 10;
                double deep = 6;

                machinetoppoint[0] = new Point3D(-width, 10.5 + _UpDownToppartPosition, -deep);
                machinetoppoint[1] = new Point3D(width, 10.5 + _UpDownToppartPosition, -deep);
                machinetoppoint[2] = new Point3D(width, 10.5 + _UpDownToppartPosition, deep);
                machinetoppoint[3] = new Point3D(-width, 10.5 + _UpDownToppartPosition, deep);
                machinetoppoint[4] = new Point3D(-width, 15.8 + _UpDownToppartPosition, -deep);
                machinetoppoint[5] = new Point3D(width, 15.8 + _UpDownToppartPosition, -deep);
                machinetoppoint[6] = new Point3D(width, 15.8 + _UpDownToppartPosition, deep);
                machinetoppoint[7] = new Point3D(-width, 15.8 + _UpDownToppartPosition, deep);

                machinebottompoint[0] = new Point3D(-width, -10.5, -deep);
                machinebottompoint[1] = new Point3D(width, -10.5, -deep);
                machinebottompoint[2] = new Point3D(width, -10.5, deep);
                machinebottompoint[3] = new Point3D(-width, -10.5, deep);
                machinebottompoint[4] = new Point3D(-width, -4, -deep);
                machinebottompoint[5] = new Point3D(width, -4, -deep);
                machinebottompoint[6] = new Point3D(width, -4, deep);
                machinebottompoint[7] = new Point3D(-width, -4, deep);

                if (!_IsImageTexture)
                {
                    machinegroup.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.MachineTopBottom], machinetoppoint));
                    machinegroup.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.MachineTopBottom], machinebottompoint));
                }
                else
                {
                    string[] toppicnames = new string[6];
                    toppicnames[0] = _ImagePath + "MechTop_F.png";
                    toppicnames[1] = _ImagePath + "MechTop_B.png";
                    toppicnames[2] = _ImagePath + "MechTop_L.png";
                    toppicnames[3] = _ImagePath + "MechTop_R.png";
                    toppicnames[4] = _ImagePath + "PostLeft_LR.png";
                    toppicnames[5] = _ImagePath + "PostLeft_LR.png";

                    string[] botpicnames = new string[6];
                    botpicnames[0] = _ImagePath + "MechBottom_F.png";
                    botpicnames[1] = _ImagePath + "MechBottom_B.png";
                    botpicnames[2] = _ImagePath + "MechBottom_L.png";
                    botpicnames[3] = _ImagePath + "MechBottom_R.png";
                    botpicnames[4] = _ImagePath + "PostLeft_LR.png";
                    botpicnames[5] = _ImagePath + "PostLeft_LR.png";

                    machinegroup.Children.Add(squaremodel.Create(toppicnames, machinetoppoint, false));
                    machinegroup.Children.Add(squaremodel.Create(botpicnames, machinebottompoint, false));
                }
                machinegroup.Children[1].Transform = new Transform3DGroup();
                _Log4NetClass.ShowInfo("Create MachinePart Model3D Success", "CreateMachineTopBottomPart");
                return machinegroup;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreateMachineTopBottomPart");
                throw ex;
            }

        }

        /// <summary>
        /// machine post part
        /// </summary>
        /// <returns></returns>
        private Model3DGroup CreateMachinePostPart()
        {
            try
            {
                double toppoint = 10.5;
                double bottompoint = -4;
                SquareModel squaremodel = new SquareModel();
                Model3DGroup machinepostgroup = new Model3DGroup();
                Model3DGroup machinepostlinegroup = new Model3DGroup();
                Point3D[] machinepost1 = new Point3D[8];
                Point3D[] machinepost2 = new Point3D[8];
                Point3D[] machinepost3 = new Point3D[8];
                Point3D[] machinepost4 = new Point3D[8];
                double x1 = 10;//8.5;
                double x2 = 6.5;
                double z1 = 3.95;
                double z2 = 6;

                machinepost1[0] = new Point3D(-x1, bottompoint, z1);
                machinepost1[1] = new Point3D(-x2 - .1, bottompoint, z1);
                machinepost1[2] = new Point3D(-x2 - .1, bottompoint, z2);
                machinepost1[3] = new Point3D(-x1, bottompoint, z2);
                machinepost1[4] = new Point3D(-x1, toppoint + _UpDownToppartPosition, z1);
                machinepost1[5] = new Point3D(-x2 - .1, toppoint + _UpDownToppartPosition, z1);
                machinepost1[6] = new Point3D(-x2 - .1, toppoint + _UpDownToppartPosition, z2);
                machinepost1[7] = new Point3D(-x1, toppoint + _UpDownToppartPosition, z2);

                machinepost2[0] = new Point3D(-x1, bottompoint, -z2);
                machinepost2[1] = new Point3D(-x2 - .1, bottompoint, -z2);
                machinepost2[2] = new Point3D(-x2 - .1, bottompoint, -z1);
                machinepost2[3] = new Point3D(-x1, bottompoint, -z1);
                machinepost2[4] = new Point3D(-x1, toppoint + _UpDownToppartPosition, -z2);
                machinepost2[5] = new Point3D(-x2 - .1, toppoint + _UpDownToppartPosition, -z2);
                machinepost2[6] = new Point3D(-x2 - .1, toppoint + _UpDownToppartPosition, -z1);
                machinepost2[7] = new Point3D(-x1, toppoint + _UpDownToppartPosition, -z1);

                machinepost3[0] = new Point3D(x2 + .1, bottompoint, -z2);
                machinepost3[1] = new Point3D(x1, bottompoint, -z2);
                machinepost3[2] = new Point3D(x1, bottompoint, -z1);
                machinepost3[3] = new Point3D(x2 + .1, bottompoint, -z1);
                machinepost3[4] = new Point3D(x2 + .1, toppoint + _UpDownToppartPosition, -z2);
                machinepost3[5] = new Point3D(x1, toppoint + _UpDownToppartPosition, -z2);
                machinepost3[6] = new Point3D(x1, toppoint + _UpDownToppartPosition, -z1);
                machinepost3[7] = new Point3D(x2 + .1, toppoint + _UpDownToppartPosition, -z1);

                machinepost4[0] = new Point3D(x2 - .1, bottompoint, z1);
                machinepost4[1] = new Point3D(x1, bottompoint, z1);
                machinepost4[2] = new Point3D(x1, bottompoint, z2);
                machinepost4[3] = new Point3D(x2 - .1, bottompoint, z2);
                machinepost4[4] = new Point3D(x2 - .1, toppoint + _UpDownToppartPosition, z1);
                machinepost4[5] = new Point3D(x1, toppoint + _UpDownToppartPosition, z1);
                machinepost4[6] = new Point3D(x1, toppoint + _UpDownToppartPosition, z2);
                machinepost4[7] = new Point3D(x2 - .1, toppoint + _UpDownToppartPosition, z2);
                if (!_IsImageTexture)
                {
                    machinepostgroup.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.MachinePost], machinepost1));
                    machinepostgroup.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.MachinePost], machinepost2));
                    machinepostgroup.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.MachinePost], machinepost3));
                    machinepostgroup.Children.Add(squaremodel.Create(_PartColors[(int)MachinePart.MachinePost], machinepost4));
                    machinepostgroup.Children.Add(machinepostlinegroup);
                }
                else
                {
                    string[] postpicleft = new string[6];
                    postpicleft[0] = _ImagePath + "PostLeft_F.png";
                    postpicleft[1] = _ImagePath + "PostRight_B.png";
                    postpicleft[2] = _ImagePath + "PostRight_LR.png";
                    postpicleft[3] = _ImagePath + "PostRight_LR.png";
                    postpicleft[4] = _ImagePath + "PostLeft_F.png";
                    postpicleft[5] = _ImagePath + "PostLeft_F.png";

                    string[] postpicleft2 = new string[6];
                    postpicleft2[0] = _ImagePath + "PostLeft_F.png";
                    postpicleft2[1] = _ImagePath + "PostRight_B.png";
                    postpicleft2[2] = _ImagePath + "PostLeft_LR.png";
                    postpicleft2[3] = _ImagePath + "PostLeft_LR.png";
                    postpicleft2[4] = _ImagePath + "PostLeft_F.png";
                    postpicleft2[5] = _ImagePath + "PostLeft_F.png";

                    string[] postpicright = new string[6];
                    postpicright[0] = _ImagePath + "PostRight_F.png";
                    postpicright[1] = _ImagePath + "PostLeft_B.png";
                    postpicright[2] = _ImagePath + "PostLeft_LR.png";
                    postpicright[3] = _ImagePath + "PostLeft_LR.png";
                    postpicright[4] = _ImagePath + "PostRight_F.png";
                    postpicright[5] = _ImagePath + "PostRight_F.png";

                    string[] postpicright2 = new string[6];
                    postpicright2[0] = _ImagePath + "PostRight_F.png";
                    postpicright2[1] = _ImagePath + "PostLeft_B.png";
                    postpicright2[2] = _ImagePath + "PostRight_LR.png";
                    postpicright2[3] = _ImagePath + "PostRight_LR.png";
                    postpicright2[4] = _ImagePath + "PostRight_F.png";
                    postpicright2[5] = _ImagePath + "PostRight_F.png";

                    //machinepostgroup.Children.Add(squaremodel.CreatePosts(machinetexture, machinepost1, false, ptexture0, ptexture1));
                    //machinepostgroup.Children.Add(squaremodel.CreatePosts(machinetexture, machinepost2, false, ptexture0, ptexture1));
                    //machinepostgroup.Children.Add(squaremodel.CreatePosts(machinetexture, machinepost3, false, ptexture0, ptexture1));
                    //machinepostgroup.Children.Add(squaremodel.CreatePosts(machinetexture, machinepost4, false, ptexture0, ptexture1));
                    machinepostgroup.Children.Add(squaremodel.Create(postpicleft, machinepost1, false));
                    machinepostgroup.Children.Add(squaremodel.Create(postpicleft2, machinepost2, false));
                    machinepostgroup.Children.Add(squaremodel.Create(postpicright2, machinepost3, false));
                    machinepostgroup.Children.Add(squaremodel.Create(postpicright, machinepost4, false));
                    machinepostgroup.Children.Add(machinepostlinegroup);

                }

                machinepostgroup.Transform = new Transform3DGroup();
                machinepostlinegroup.Transform = new Transform3DGroup();

                _Log4NetClass.ShowInfo("Create MachinePostPart Model3D Success", "CreateMachinePostPart");
                return machinepostgroup;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreateMachinePostPart");
                throw ex;
            }

        }

        /// <summary>
        /// add translatetranform 3D 
        /// </summary>
        /// <param name="trans3dGroup">tranfrom3Dgroup</param>
        /// <param name="transValue">tranform Y value</param>
        private void AddTranslateTransform3D(Transform3DGroup trans3dGroup, double transValue)
        {
            try
            {
                bool ret = false;
                for (int i = 0; i < trans3dGroup.Children.Count; i++)
                {
                    TranslateTransform3D tran3d = trans3dGroup.Children[i] as TranslateTransform3D;
                    if (tran3d != null)
                    {
                        tran3d = new TranslateTransform3D(0, transValue, 0);
                        trans3dGroup.Children[i] = tran3d;
                        ret = true;
                        break;
                    }
                }


                if (!ret)
                {
                    TranslateTransform3D tran3d = new TranslateTransform3D(0, transValue, 0);
                    trans3dGroup.Children.Add(tran3d);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "AddTranslateTransform3D");
                throw ex;
            }
        }

        /// <summary>
        /// add scale tranform3D
        /// </summary>
        /// <param name="trans3dGroup">transform3dgroup</param>
        /// <param name="transValue">scale Y value</param>
        private void AddScaleTransform3D(Transform3DGroup trans3dGroup, double transValue)
        {
            try
            {
                bool ret = false;
                for (int i = 0; i < trans3dGroup.Children.Count; i++)
                {
                    ScaleTransform3D scale3d = trans3dGroup.Children[i] as ScaleTransform3D;
                    if (scale3d != null)
                    {
                        scale3d = new ScaleTransform3D(1, transValue, 1);
                        trans3dGroup.Children[i] = scale3d;
                        ret = true;
                        break;
                    }
                }


                if (!ret)
                {
                    ScaleTransform3D scale3d = new ScaleTransform3D(1, transValue, 1);
                    trans3dGroup.Children.Add(scale3d);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "AddScaleTransform3D");
                throw ex;
            }

        }

        /// <summary>
        /// add scale tranform3D
        /// </summary>
        /// <param name="trans3dGroup">transform3dgroup</param>
        /// <param name="scale">Vector3D Scale</param>
        /// <param name="position">Point3D Position</param>
        private void AddScaleTransform3D(Transform3DGroup trans3dGroup, Vector3D scale, Point3D position)
        {
            try
            {
                bool ret = false;
                for (int i = 0; i < trans3dGroup.Children.Count; i++)
                {
                    ScaleTransform3D scale3d = trans3dGroup.Children[i] as ScaleTransform3D;
                    if (scale3d != null)
                    {
                        scale3d = new ScaleTransform3D(scale, position);
                        trans3dGroup.Children[i] = scale3d;
                        ret = true;
                        break;
                    }
                }


                if (!ret)
                {
                    ScaleTransform3D scale3d = new ScaleTransform3D(scale, position);
                    trans3dGroup.Children.Add(scale3d);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "AddScaleTransform3D2");
                throw ex;
            }

        }

        /// <summary>
        /// add scale tranform3D
        /// </summary>
        /// <param name="trans3dGroup">transform3dgroup</param>
        /// <param name="scale">Vector3D Scale</param>
        /// <param name="position">Point3D Position</param>
        private void AddScaleTransform3D(Transform3DGroup trans3dGroup, Vector3D scale)
        {
            try
            {
                bool ret = false;
                for (int i = 0; i < trans3dGroup.Children.Count; i++)
                {
                    ScaleTransform3D scale3d = trans3dGroup.Children[i] as ScaleTransform3D;
                    if (scale3d != null)
                    {
                        scale3d = new ScaleTransform3D(scale);
                        trans3dGroup.Children[i] = scale3d;
                        ret = true;
                        break;
                    }
                }


                if (!ret)
                {
                    ScaleTransform3D scale3d = new ScaleTransform3D(scale);
                    trans3dGroup.Children.Add(scale3d);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "AddScaleTransform3D3");
                throw ex;
            }

        }


        /// <summary>
        /// set tranperent each object
        /// </summary>
        /// <param name="modelTrans"></param>
        /// <param name="transparentValue"></param>
        private void SetTransparent(Model3DGroup modelTrans, double transparentValue)
        {
            try
            {
                GeometryModel3D geo3d1 = ((Model3DGroup)modelTrans).Children[0] as GeometryModel3D;
                GeometryModel3D geo3d2 = ((Model3DGroup)modelTrans).Children[1] as GeometryModel3D;

                DiffuseMaterial diffmat = geo3d1.Material as DiffuseMaterial;
                Brush brush = diffmat.Brush;
                brush.Opacity = transparentValue;

                DiffuseMaterial diffmat2 = geo3d2.Material as DiffuseMaterial;
                Brush brush2 = diffmat2.Brush;
                brush2.Opacity = transparentValue;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "SetTransparent");
                throw ex;
            }
        }

        #endregion

        #region Public Function
        /// <summary>
        /// create model
        /// </summary>
        /// <returns></returns>
        public ModelVisual3D Create()
        {
            try
            {
                ModelVisual3D outputmodel = new ModelVisual3D();

                Model3DGroup outputgroup = new Model3DGroup();
                Model3DGroup machinegroup = CreateMachineTopBottomPart();
                Model3DGroup toppartgroup = CreateTopPart();
                Model3DGroup bottompartgroup = CreateBottomPart();
                Model3DGroup machinepostgroup = CreateMachinePostPart();

                outputgroup.Children.Add(toppartgroup);
                outputgroup.Children.Add(bottompartgroup);
                outputgroup.Children.Add(machinegroup);
                outputgroup.Children.Add(machinepostgroup);

                outputgroup.Transform = new Transform3DGroup();
                outputmodel = new ModelVisual3D();
                outputmodel.Content = outputgroup;
                _CurrentModel = outputmodel;
                _TopPlateBasePos = this.UpperPlatePos;

                _Log4NetClass.ShowInfo("Create Model3D Success", "Create");
                return outputmodel;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Create");
                throw ex;
            }
        }

        /// <summary>
        /// scale stripper plate (show/hide)
        /// </summary>
        /// <param name="scaleSize">point scale size</param>
        public void ScaleStripperPlate(Point3D scaleSize)
        {
            try
            {

                if (_CurrentModel != null)
                {
                    Model3DGroup upperpart = this.UpperPartModel3DGroup;

                    Model3DGroup upperpartmodelgroup = upperpart.Children[0] as Model3DGroup;
                    Model3DGroup stripper = upperpartmodelgroup.Children[3] as Model3DGroup;

                    if (stripper != null)
                    {
                        Transform3DGroup topplatetran = stripper.Transform as Transform3DGroup;
                        Vector3D scale = new Vector3D(scaleSize.X, scaleSize.Y, scaleSize.Z);
                        Point3D point = new Point3D(_TopPlateBasePos.X, _TopPlateBasePos.Y, _TopPlateBasePos.Z);

                        AddScaleTransform3D(topplatetran, scale, point);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// scale Machine Post (show/hide)
        /// </summary>
        /// <param name="scaleSize">point scale size</param>
        public void ScaleMachinePost(Point3D scaleSize)
        {
            try
            {
                if (_CurrentModel != null)
                {
                    Model3DGroup machinepost = ((Model3DGroup)_CurrentModel.Content).Children[3] as Model3DGroup;

                    if (machinepost != null)
                    {
                        Transform3DGroup machineposttran = machinepost.Transform as Transform3DGroup;
                        Vector3D scale = new Vector3D(scaleSize.X, scaleSize.Y, scaleSize.Z);
                        AddScaleTransform3D(machineposttran, scale);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Set Transperent to Machine Object
        /// </summary>
        /// <param name="transparentValue">double 0 to 1</param>
        public void TransparentMachine(double transparentValue)
        {
            if (_CurrentModel != null)
            {
                Model3DGroup machinetopbottom = ((Model3DGroup)_CurrentModel.Content).Children[2] as Model3DGroup;
                Model3DGroup machinepostpart = ((Model3DGroup)_CurrentModel.Content).Children[3] as Model3DGroup;

                Model3DGroup machinetop = ((Model3DGroup)machinetopbottom.Children[0]);
                Model3DGroup machinebottom = ((Model3DGroup)machinetopbottom.Children[1]);
                Model3DGroup machinepost1 = ((Model3DGroup)machinepostpart.Children[0]);
                Model3DGroup machinepost2 = ((Model3DGroup)machinepostpart.Children[1]);
                Model3DGroup machinepost3 = ((Model3DGroup)machinepostpart.Children[2]);
                Model3DGroup machinepost4 = ((Model3DGroup)machinepostpart.Children[3]);

                for (int i = 0; i < machinetop.Children.Count; i++)
                {
                    SetTransparent((Model3DGroup)machinetop.Children[i], transparentValue);
                    SetTransparent((Model3DGroup)machinebottom.Children[i], transparentValue);
                    SetTransparent((Model3DGroup)machinepost1.Children[i], transparentValue);
                    SetTransparent((Model3DGroup)machinepost2.Children[i], transparentValue);
                    SetTransparent((Model3DGroup)machinepost3.Children[i], transparentValue);
                    SetTransparent((Model3DGroup)machinepost4.Children[i], transparentValue);

                }
            }
        }

        /// <summary>
        /// scale top/bottom part (small/normal/large)
        /// </summary>
        /// <param name="scaleSize">point scale size</param>
        public void ScaleTopBottomPart(Point3D scaleSize)
        {
            try
            {
                if (_CurrentModel != null)
                {
                    Model3DGroup upperpart = this.UpperPartModel3DGroup;
                    Model3DGroup lowerpart = this.LowerPartModel3DGroup;

                    Model3DGroup machinetopbottom = ((Model3DGroup)_CurrentModel.Content).Children[2] as Model3DGroup;
                    Model3DGroup machinepostpart = ((Model3DGroup)_CurrentModel.Content).Children[3] as Model3DGroup;

                    if (upperpart != null && lowerpart != null && machinetopbottom != null)
                    {
                        Model3DGroup topplatemodel = ((Model3DGroup)upperpart.Children[0]);
                        Model3DGroup topshaftmodel = ((Model3DGroup)upperpart.Children[1]);
                        Model3DGroup bottomplatemodel = ((Model3DGroup)lowerpart.Children[0]);
                        Model3DGroup machinebottom = ((Model3DGroup)machinetopbottom.Children[1]);
                        Model3DGroup machinepost = ((Model3DGroup)machinepostpart.Children[4]);

                        Transform3DGroup topplatetran = topplatemodel.Transform as Transform3DGroup;
                        Transform3DGroup topshafttran = topshaftmodel.Transform as Transform3DGroup;
                        Transform3DGroup bottomplatetran = bottomplatemodel.Transform as Transform3DGroup;
                        Transform3DGroup machinebottomtran = machinebottom.Transform as Transform3DGroup;
                        Transform3DGroup machineposttran = machinepost.Transform as Transform3DGroup;

                        Vector3D scale = new Vector3D(scaleSize.X, scaleSize.Y, scaleSize.Z);
                        Point3D topplatepos = new Point3D(0, -2.5, 0);
                        //scale top
                        AddScaleTransform3D(topplatetran, scale, topplatepos);


                        Point3D bottomplatepos = new Point3D(0, -3, 0);
                        //scale bottom
                        AddScaleTransform3D(bottomplatetran, scale, bottomplatepos);

                        double topshafttrans = 0;
                        double machinebotval = 0;
                        double machinepostscaleval = 0;
                        double machineposttranval = 0;



                        if (scaleSize.Y < 1)
                        {
                            topshafttrans = -3;
                            machinebotval = 0.6;
                            machinepostscaleval = 0.97;
                            machineposttranval = 0.35;

                        }
                        else if (scaleSize.Y > 1)
                        {
                            topshafttrans = 0;
                            machinebotval = -0.7;
                            machinepostscaleval = 1;
                            machineposttranval = 0;
                        }
                        else
                        {
                            topshafttrans = 0;
                            machinebotval = 0;
                            machinepostscaleval = 1;
                            machineposttranval = 0;
                        }


                        //shaft translate position
                        TranslateTransform3D tran3d = null;
                        if (topshafttran.Children.Count == 1)
                        {
                            tran3d = new TranslateTransform3D(0, topshafttrans, 0);
                            topshafttran.Children.Add(tran3d);
                        }
                        else if (topshafttran.Children.Count == 2)
                        {
                            tran3d = topshafttran.Children[1] as TranslateTransform3D;
                            if (tran3d != null)
                                tran3d.OffsetY = topshafttrans;
                        }


                        //machine bottom trans                                          
                        AddTranslateTransform3D(machinebottomtran, machinebotval);

                        //machine post line 
                        AddScaleTransform3D(machineposttran, machinepostscaleval);
                        AddTranslateTransform3D(machineposttran, machineposttranval);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// rotate model by mouse
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void RotateObjectByMouse(double dx, double dy)
        {
            try
            {
                double mouseAngle = 0;
                if (dx != 0 && dy != 0)
                {
                    mouseAngle = Math.Asin(Math.Abs(dy) / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
                    if (dx < 0 && dy > 0) mouseAngle += Math.PI / 2;
                    else if (dx < 0 && dy < 0) mouseAngle += Math.PI;
                    else if (dx > 0 && dy < 0) mouseAngle += Math.PI * 1.5;
                }
                else if (dx == 0 && dy != 0) mouseAngle = Math.Sign(dy) > 0 ? Math.PI / 2 : Math.PI * 1.5;
                else if (dx != 0 && dy == 0) mouseAngle = Math.Sign(dx) > 0 ? 0 : Math.PI;

                double axisAngle = mouseAngle + Math.PI / 2;

                Vector3D axis = new Vector3D(Math.Cos(axisAngle) * 4, Math.Sin(axisAngle) * 4, 0);

                double rotation = 0.01 * Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                rotation = rotation * 180 / Math.PI;

                this.RotateObjectByAngle(axis, rotation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// rotate model by angle
        /// </summary>
        /// <param name="axis">axis for rotate</param>
        /// <param name="rotateAngle">rotation angle</param>
        public void RotateObjectByAngle(Vector3D axis, double rotateAngle)
        {
            try
            {
                if (_CurrentModel != null)
                {
                    Transform3DGroup group = _CurrentModel.Content.Transform as Transform3DGroup;


                    if (group.Children.Count > 0)
                    {
                        for (int i = 0; i < group.Children.Count; i++)
                        {
                            MatrixTransform3D rtrans = group.Children[i] as MatrixTransform3D;
                            if (rtrans != null)
                            {

                                Matrix3D matrix = rtrans.Matrix;
                                matrix.Rotate(new Quaternion(axis, rotateAngle));

                                //get current radius of X axis
                                double rotaterad = Math.Acos(matrix.M22);

                                if ((rotaterad * (180 / Math.PI)) > _MaxRotateXDegree)
                                {
                                    matrix.Rotate(new Quaternion(axis, -rotateAngle));
                                }

                                MatrixTransform3D mat = new MatrixTransform3D(matrix);

                                rtrans = mat;
                                group.Children[i] = rtrans;
                            }
                        }
                    }
                    else
                    {
                        Matrix3D matrix = new Matrix3D();
                        matrix.Rotate(new Quaternion(axis, rotateAngle));
                        MatrixTransform3D mat = new MatrixTransform3D(matrix);
                        group.Children.Add(mat);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// reset model position
        /// </summary>
        public void ResetPosition()
        {
            try
            {
                if (_CurrentModel != null)
                {
                    _CurrentModel.Content.Transform = new Transform3DGroup();
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
