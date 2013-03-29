using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Windows.Media.Animation;

namespace Graph3DLib.Model
{
    /// <summary>
    /// class for create cylinder model
    /// </summary>
    public class CylinderModel
    {
        #region Private Const

        /// <summary>
        /// radius of cylinder
        /// </summary>
        private const double _CylinderRadius = 1.0;

        #endregion

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        public CylinderModel()
        {

        }
        #endregion

        #region Private Function
        /// <summary>
        /// get position
        /// </summary>
        /// <param name="t">t angle</param>
        /// <param name="y">y</param>
        /// <param name="rad">radius</param>
        /// <param name="startPoint">start location</param>
        /// <returns></returns>
        private Point3D GetPosition(double t, double y, double rad, Point3D startPoint)
        {
            double x = (rad * Math.Cos(t)) + startPoint.X;
            double z = (rad * Math.Sin(t)) + startPoint.Z;

            return new Point3D(x, y + startPoint.Y, z);
        }

        /// <summary>
        /// get position for 3D circle
        /// </summary>
        /// <param name="t"></param>
        /// <param name="rad">radius</param>
        /// <param name="startPoint">start location</param>
        /// <returns></returns>
        private Point3D GetPositionCirCle(double t, double rad, Point3D startPoint)
        {
            double x = (rad * Math.Cos(t)) + startPoint.X;
            double z = (rad * Math.Sin(t)) + startPoint.Z;

            return new Point3D(x, startPoint.Y, z);
        }

        private Vector3D GetNormal(double t, double y)
        {
            double x = Math.Cos(t);
            double z = Math.Sin(t);

            return new Vector3D(x, 0, z);
        }

        /// <summary>
        /// convert degree to rad
        /// </summary>
        /// <param name="degrees">input degree</param>
        /// <returns></returns>
        private double DegToRad(double degrees)
        {
            return (degrees / 180.0) * Math.PI;
        }

        /// <summary>
        /// get texture coordinate
        /// </summary>
        /// <param name="t"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Point GetTextureCoordinate(double t, double y)
        {
            return new Point(1.0 - t * 1 / (2 * Math.PI), y * -0.5 + 0.5);
        }

        /// <summary>
        /// create 2D circle
        /// </summary>
        /// <param name="modelColor">color</param>
        /// <param name="startPoint">start point</param>
        /// <param name="radius">radius</param>
        /// <param name="normal">normal vector</param>
        /// <returns></returns>
        private GeometryModel3D CreateCircle(Brush modelColor, Point3D startPoint, double radius, Vector3D normal)
        {
            try
            {
                Material material = new DiffuseMaterial(modelColor);

                GeometryModel3D triangleModel = new GeometryModel3D(
                    CircleMesh(normal, startPoint, radius), material);

                triangleModel.Freeze();
                return triangleModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create cylinder 3D
        /// </summary>
        /// <param name="modelColor">color</param>
        /// <param name="startPoint">start point</param>
        /// <param name="radius">radius</param>
        /// <param name="maxHigh">max high of cylinder</param>
        /// <returns></returns>
        private GeometryModel3D CreateCylinder(Brush modelColor, Point3D startPoint, double radius, double maxHigh)
        {
            try
            {
                Material material = new DiffuseMaterial(modelColor);

                GeometryModel3D triangleModel = new GeometryModel3D(
                    CylinderMech(32, 32, radius, startPoint, maxHigh), material);

                triangleModel.Freeze();
                return triangleModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create cylinder mech geometry 3D
        /// </summary>
        /// <param name="tDiv"></param>
        /// <param name="yDiv"></param>
        /// <param name="rad"></param>
        /// <param name="startPoint"></param>
        /// <param name="maxHigh"></param>
        /// <returns></returns>
        private MeshGeometry3D CylinderMech(int tDiv, int yDiv, double rad, Point3D startPoint, double maxHigh)
        {
            try
            {
                double maxTheta = (360 / 180.0) * Math.PI;
                double minY = 0.0;
                double maxY = maxHigh;

                double dt = maxTheta / tDiv;
                double dy = (maxY - minY) / yDiv;

                MeshGeometry3D mesh = new MeshGeometry3D();

                for (int yi = 0; yi <= yDiv; yi++)
                {
                    double y = minY + yi * dy;

                    for (int ti = 0; ti <= tDiv; ti++)
                    {
                        double t = ti * dt;

                        mesh.Positions.Add(GetPosition(t, y, rad, startPoint));
                        mesh.Normals.Add(GetNormal(t, y));
                        mesh.TextureCoordinates.Add(GetTextureCoordinate(t, y));

                        int x0 = ti;
                        int x1 = (ti + 1);
                        int y0 = yi * (tDiv + 1);
                        int y1 = (yi + 1) * (tDiv + 1);

                        mesh.TriangleIndices.Add(x0 + y0);
                        mesh.TriangleIndices.Add(x0 + y1);
                        mesh.TriangleIndices.Add(x1 + y0);

                        mesh.TriangleIndices.Add(x1 + y0);
                        mesh.TriangleIndices.Add(x0 + y1);
                        mesh.TriangleIndices.Add(x1 + y1);
                    }
                }

                mesh.Freeze();
                return mesh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create circle 2D mesh geomatry 3D
        /// </summary>
        /// <param name="inpNormal"></param>
        /// <param name="startPoint"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private MeshGeometry3D CircleMesh(Vector3D inpNormal, Point3D startPoint, double radius)
        {
            try
            {
                double maxTheta = (360 / 180.0) * Math.PI;
                double dt = maxTheta / 32;
                int point1 = 0;
                int point2 = 0;

                Point3D center = startPoint;

                MeshGeometry3D mesh = new MeshGeometry3D();
                mesh.Positions.Add(center);

                if (inpNormal.Y == 1)
                {
                    point1 = 2;
                    point2 = 1;
                }
                else if (inpNormal.Y == -1)
                {
                    point1 = 1;
                    point2 = 2;
                }

                for (int ti = 0; ti <= 360; ti++)
                {
                    double t = ti * dt;

                    if (ti % 3 == 0 && ti > 0)
                    {
                        mesh.TriangleIndices.Add(0);
                        mesh.TriangleIndices.Add(ti + point1);
                        mesh.TriangleIndices.Add(ti + point2);
                        mesh.Normals.Add(inpNormal);
                    }

                    mesh.Positions.Add(GetPositionCirCle(t, radius, startPoint));
                }


                mesh.Freeze();
                return mesh;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Public Function

        /// <summary>
        /// create cylider 3D model
        /// </summary>
        /// <param name="modelColor">color</param>
        /// <param name="startPos">start position</param>
        /// <param name="maxHigh">max high</param>
        /// <returns></returns>
        public Model3DGroup Create(Color modelColor, Point3D startPos, double maxHigh)
        {
            try
            {
                SolidColorBrush modelbrush = new SolidColorBrush(modelColor);
                Model3DGroup cube = new Model3DGroup();
                Point3D uppercircle = startPos;
                modelbrush.Freeze();
                uppercircle.Y = startPos.Y + maxHigh;
                cube.Children.Add(CreateCircle(modelbrush, uppercircle, _CylinderRadius, new Vector3D(0, 1, 0)));
                cube.Children.Add(CreateCircle(modelbrush, startPos, _CylinderRadius, new Vector3D(0, -1, 0)));
                cube.Children.Add(CreateCylinder(modelbrush, startPos, _CylinderRadius, maxHigh));
                return cube;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
