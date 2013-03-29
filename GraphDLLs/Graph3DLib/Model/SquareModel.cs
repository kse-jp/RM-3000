using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;

namespace Graph3DLib.Model
{
    /// <summary>
    /// square model class
    /// </summary>
    public class SquareModel
    {
        #region Private Function
        /// <summary>
        /// Create image brush for picture image.
        /// </summary>
        /// <param name="pictureName"></param>
        /// <param name="isFreeze"></param>
        /// <returns></returns>
        private ImageBrush CreateImageBrush(string pictureName, bool isFreeze)
        {
            try
            {
                ImageBrush imagebrush = null;
                if (pictureName != string.Empty)
                {
                    Uri inpuri = new Uri(@pictureName, UriKind.Relative);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = inpuri;
                    bi.EndInit();
                    imagebrush = new ImageBrush(bi);
                    imagebrush.Opacity = 1;
                    //imagebrush.Stretch = Stretch.UniformToFill;   
                    imagebrush.ViewportUnits = BrushMappingMode.Absolute;
                    imagebrush.TileMode = TileMode.Tile;
                    if (isFreeze)
                    {
                        imagebrush.Freeze();
                    }
                }
                return imagebrush;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// create triangle
        /// </summary>
        /// <param name="p0">point0</param>
        /// <param name="p1">point1</param>
        /// <param name="p2">point2</param>
        /// <returns></returns>
        private MeshGeometry3D CreateTriangle(Point3D p0, Point3D p1, Point3D p2)
        {
            try
            {
                MeshGeometry3D triangleMesh = new MeshGeometry3D();
                triangleMesh.Positions.Add(p0);
                triangleMesh.Positions.Add(p1);
                triangleMesh.Positions.Add(p2);
                triangleMesh.TriangleIndices.Add(0);
                triangleMesh.TriangleIndices.Add(1);
                triangleMesh.TriangleIndices.Add(2);
                Vector3D normal = CreateNormal(p0, p1, p2);

                triangleMesh.Normals.Add(normal);
                triangleMesh.Freeze();
                return triangleMesh;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private MeshGeometry3D CreateTriangle(Point3D p0, Point3D p1, Point3D p2, Point[] texturePoint)
        {
            try
            {
                MeshGeometry3D triangleMesh = new MeshGeometry3D();
                triangleMesh.Positions.Add(p0);
                triangleMesh.Positions.Add(p1);
                triangleMesh.Positions.Add(p2);
                triangleMesh.TriangleIndices.Add(0);
                triangleMesh.TriangleIndices.Add(1);
                triangleMesh.TriangleIndices.Add(2);
                Vector3D normal = CreateNormal(p0, p1, p2);

                triangleMesh.TextureCoordinates.Add(texturePoint[0]);
                triangleMesh.TextureCoordinates.Add(texturePoint[1]);
                triangleMesh.TextureCoordinates.Add(texturePoint[2]);



                triangleMesh.Normals.Add(normal);
                triangleMesh.Freeze();
                return triangleMesh;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MeshGeometry3D CreateTriangle2D(Point3D p0, Point3D p1, Point3D p2, Point[] texturePoint)
        {
            try
            {
                MeshGeometry3D triangleMesh = new MeshGeometry3D();

                triangleMesh.Positions.Add(p0);
                triangleMesh.Positions.Add(p1);
                triangleMesh.Positions.Add(p2);
                triangleMesh.TriangleIndices.Add(0);
                triangleMesh.TriangleIndices.Add(1);
                triangleMesh.TriangleIndices.Add(2);
                Vector3D normal = CreateNormal(p0, p1, p2);

                triangleMesh.TextureCoordinates.Add(texturePoint[0]);
                triangleMesh.TextureCoordinates.Add(texturePoint[1]);
                triangleMesh.TextureCoordinates.Add(texturePoint[2]);

                triangleMesh.Normals.Add(normal);
                triangleMesh.Freeze();
                return triangleMesh;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// create normal
        /// </summary>
        /// <param name="p0">point0</param>
        /// <param name="p1">point1</param>
        /// <param name="p2">point2</param>
        /// <returns></returns>
        private Vector3D CreateNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            try
            {
                Vector3D vector0 = new Vector3D();
                Vector3D vector1 = new Vector3D();
                vector0.X = p1.X - p0.X;
                vector0.Y = p1.Y - p0.Y;
                vector0.Z = p1.Z - p0.Z;

                vector1.X = p2.X - p1.X;
                vector1.Y = p2.Y - p1.Y;
                vector1.Z = p2.Z - p1.Z;

                return Vector3D.CrossProduct(vector0, vector1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create geomatry model 3D
        /// </summary>
        /// <param name="p0">point0</param>
        /// <param name="p1">point1</param>
        /// <param name="p2">point2</param>
        /// <param name="modelColor"></param>
        /// <returns></returns>
        private GeometryModel3D CreateGeoModel3D(Point3D p0, Point3D p1, Point3D p2, Brush modelColor)
        {
            try
            {
                Material material = new DiffuseMaterial(modelColor);

                GeometryModel3D triangleModel = new GeometryModel3D(
                    CreateTriangle(p0, p1, p2), material);


                return triangleModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private GeometryModel3D CreateGeoModel3D(Point3D p0, Point3D p1, Point3D p2, Point[] texturePoint, Brush modelColor)
        {
            try
            {
                Material material = new DiffuseMaterial(modelColor);

                GeometryModel3D triangleModel = new GeometryModel3D(
                    CreateTriangle(p0, p1, p2, texturePoint), material);


                return triangleModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create geomatry model 3D for Label picture
        /// </summary>
        /// <param name="p0">point0</param>
        /// <param name="p1">point1</param>
        /// <param name="p2">point2</param>
        /// <param name="texturePoint">texturepoint</param>
        /// <param name="modelColor">color</param>
        /// <returns></returns>
        private GeometryModel3D CreateGeoModel2D(Point3D p0, Point3D p1, Point3D p2, Point[] texturePoint, Brush modelColor)
        {
            try
            {
                Material material = new DiffuseMaterial(modelColor);

                GeometryModel3D triangleModel = new GeometryModel3D(
                    CreateTriangle2D(p0, p1, p2, texturePoint), material);

                return triangleModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private GeometryModel3D CreateGeoModel2D(Point3D p0, Point3D p1, Point3D p2, Brush modelColor)
        {
            try
            {
                Material material = new DiffuseMaterial(modelColor);

                GeometryModel3D triangleModel = new GeometryModel3D(
                    CreateTriangle(p0, p1, p2), material);

                return triangleModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region public function
        /// <summary>
        /// create square model
        /// </summary>
        /// <param name="modelColor">model color</param>
        /// <param name="points">model points</param>
        /// <returns>model3d group</returns>
        public Model3DGroup Create(Color modelColor, Point3D[] points)
        {
            try
            {
                Point[] texturepoint0 = { new Point(0, 0), new Point(0, 1), new Point(1, 0) };
                Point[] texturepoint1 = { new Point(1, 0), new Point(0, 1), new Point(1, 1) };

                SolidColorBrush modelbrush = new SolidColorBrush(modelColor);
                modelbrush.Freeze();
                Model3DGroup bottomside = new Model3DGroup();
                bottomside.Children.Add(CreateGeoModel3D(points[2], points[3], points[0], modelbrush));
                bottomside.Children.Add(CreateGeoModel3D(points[2], points[0], points[1], modelbrush));
                bottomside.Freeze();

                Model3DGroup topside = new Model3DGroup();
                topside.Children.Add(CreateGeoModel3D(points[7], points[6], points[5], modelbrush));
                topside.Children.Add(CreateGeoModel3D(points[7], points[5], points[4], modelbrush));
                topside.Freeze();

                Model3DGroup rightside = new Model3DGroup();
                rightside.Children.Add(CreateGeoModel3D(points[2], points[1], points[5], modelbrush));
                rightside.Children.Add(CreateGeoModel3D(points[2], points[5], points[6], modelbrush));
                rightside.Freeze();

                Model3DGroup leftside = new Model3DGroup();
                leftside.Children.Add(CreateGeoModel3D(points[0], points[3], points[7], modelbrush));
                leftside.Children.Add(CreateGeoModel3D(points[0], points[7], points[4], modelbrush));
                leftside.Freeze();

                Model3DGroup frontside = new Model3DGroup();
                frontside.Children.Add(CreateGeoModel3D(points[3], points[2], points[6], modelbrush));
                frontside.Children.Add(CreateGeoModel3D(points[3], points[6], points[7], modelbrush));
                frontside.Freeze();

                Model3DGroup backside = new Model3DGroup();
                backside.Children.Add(CreateGeoModel3D(points[0], points[4], points[5], modelbrush));
                backside.Children.Add(CreateGeoModel3D(points[1], points[0], points[5], modelbrush));
                backside.Freeze();


                Model3DGroup cube = new Model3DGroup();
                cube.Children.Add(bottomside);
                cube.Children.Add(topside);
                cube.Children.Add(rightside);
                cube.Children.Add(leftside);
                cube.Children.Add(frontside);
                cube.Children.Add(backside);

                return cube;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// create square model
        /// </summary>
        /// <param name="pictureName">picture file path name</param>
        /// <param name="points">model points</param>
        /// <returns>model3d group</returns>
        public Model3DGroup Create(string pictureName, Point3D[] points, bool isFreeze)
        {
            try
            {
                ImageBrush imagebrush = CreateImageBrush(pictureName, isFreeze);

                Point[] ptexture0 = { new Point(0, 1), new Point(0, 0), new Point(1, 0) };
                Point[] ptexture1 = { new Point(0, 1), new Point(1, 0), new Point(1, 1) };

                Model3DGroup bottomside = new Model3DGroup();
                bottomside.Children.Add(CreateGeoModel3D(points[2], points[3], points[0], ptexture0, imagebrush));
                bottomside.Children.Add(CreateGeoModel3D(points[2], points[0], points[1], ptexture1, imagebrush));


                Model3DGroup topside = new Model3DGroup();
                topside.Children.Add(CreateGeoModel3D(points[7], points[6], points[5], ptexture0, imagebrush));
                topside.Children.Add(CreateGeoModel3D(points[7], points[5], points[4], ptexture1, imagebrush));


                Model3DGroup rightside = new Model3DGroup();
                rightside.Children.Add(CreateGeoModel3D(points[2], points[1], points[5], ptexture0, imagebrush));
                rightside.Children.Add(CreateGeoModel3D(points[2], points[5], points[6], ptexture1, imagebrush));


                Model3DGroup leftside = new Model3DGroup();
                leftside.Children.Add(CreateGeoModel3D(points[0], points[3], points[7], ptexture0, imagebrush));
                leftside.Children.Add(CreateGeoModel3D(points[0], points[7], points[4], ptexture1, imagebrush));


                Model3DGroup frontside = new Model3DGroup();
                frontside.Children.Add(CreateGeoModel3D(points[3], points[2], points[6], ptexture0, imagebrush));
                frontside.Children.Add(CreateGeoModel3D(points[3], points[6], points[7], ptexture1, imagebrush));


                Model3DGroup backside = new Model3DGroup();
                backside.Children.Add(CreateGeoModel3D(points[5], points[1], points[0], ptexture0, imagebrush));
                backside.Children.Add(CreateGeoModel3D(points[5], points[0], points[4], ptexture1, imagebrush));


                if (isFreeze)
                {
                    bottomside.Freeze();
                    topside.Freeze();
                    rightside.Freeze();
                    leftside.Freeze();
                    frontside.Freeze();
                    backside.Freeze();
                }


                Model3DGroup cube = new Model3DGroup();
                cube.Children.Add(bottomside);
                cube.Children.Add(topside);
                cube.Children.Add(rightside);
                cube.Children.Add(leftside);
                cube.Children.Add(frontside);
                cube.Children.Add(backside);

                return cube;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create square model
        /// </summary>
        /// <param name="pictureName">picture file path name</param>
        /// <param name="points">model points</param>
        /// <returns>model3d group</returns>
        public Model3DGroup Create(string[] pictureNames, Point3D[] points, bool isFreeze)
        {
            try
            {
                //0=front,1=back,2=left,3=right,4=top,5=bottom
                ImageBrush[] imgbrushs = new ImageBrush[6];

                for (int i = 0; i < pictureNames.Length; i++)
                {
                    imgbrushs[i] = CreateImageBrush(pictureNames[i], isFreeze);
                }

                Point[] ptexture0 = { new Point(0, 1), new Point(1, 1), new Point(1, 0) };
                Point[] ptexture1 = { new Point(0, 1), new Point(1, 0), new Point(0, 0) };

                Model3DGroup bottomside = new Model3DGroup();
                bottomside.Children.Add(CreateGeoModel3D(points[2], points[3], points[0], ptexture0, imgbrushs[5]));
                bottomside.Children.Add(CreateGeoModel3D(points[2], points[0], points[1], ptexture1, imgbrushs[5]));


                Model3DGroup topside = new Model3DGroup();
                topside.Children.Add(CreateGeoModel3D(points[7], points[6], points[5], ptexture0, imgbrushs[4]));
                topside.Children.Add(CreateGeoModel3D(points[7], points[5], points[4], ptexture1, imgbrushs[4]));


                Model3DGroup rightside = new Model3DGroup();
                rightside.Children.Add(CreateGeoModel3D(points[2], points[1], points[5], ptexture0, imgbrushs[3]));
                rightside.Children.Add(CreateGeoModel3D(points[2], points[5], points[6], ptexture1, imgbrushs[3]));


                Model3DGroup leftside = new Model3DGroup();
                leftside.Children.Add(CreateGeoModel3D(points[0], points[3], points[7], ptexture0, imgbrushs[2]));
                leftside.Children.Add(CreateGeoModel3D(points[0], points[7], points[4], ptexture1, imgbrushs[2]));


                Model3DGroup frontside = new Model3DGroup();
                frontside.Children.Add(CreateGeoModel3D(points[3], points[2], points[6], ptexture0, imgbrushs[0]));
                frontside.Children.Add(CreateGeoModel3D(points[3], points[6], points[7], ptexture1, imgbrushs[0]));


                Model3DGroup backside = new Model3DGroup();
                backside.Children.Add(CreateGeoModel3D(points[1], points[0], points[4], ptexture0, imgbrushs[1]));
                backside.Children.Add(CreateGeoModel3D(points[1], points[4], points[5], ptexture1, imgbrushs[1]));


                if (isFreeze)
                {
                    bottomside.Freeze();
                    topside.Freeze();
                    rightside.Freeze();
                    leftside.Freeze();
                    frontside.Freeze();
                    backside.Freeze();
                }


                Model3DGroup cube = new Model3DGroup();
                cube.Children.Add(bottomside);
                cube.Children.Add(topside);
                cube.Children.Add(rightside);
                cube.Children.Add(leftside);
                cube.Children.Add(frontside);
                cube.Children.Add(backside);

                return cube;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create square 2D for picture label
        /// </summary>
        /// <param name="pictureName">picture name in GIF</param>
        /// <param name="points">Texture point</param>
        /// <returns></returns>
        public Model3DGroup CreatePictureLabel(string pictureName, Point3D[] points)
        {
            try
            {
                Uri inpuri = new Uri(@pictureName, UriKind.Relative);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = inpuri;
                bi.EndInit();
                ImageBrush imagebrush = new ImageBrush(bi);
                imagebrush.Opacity = 100;
                imagebrush.Freeze();


                Point[] texturepoint0 = { new Point(0, 0), new Point(0, 1), new Point(1, 0) };
                Point[] texturepoint1 = { new Point(1, 0), new Point(0, 1), new Point(1, 1) };


                Model3DGroup square = new Model3DGroup();
                square.Children.Add(CreateGeoModel2D(points[0], points[3], points[1], texturepoint0, imagebrush));
                square.Children.Add(CreateGeoModel2D(points[1], points[3], points[2], texturepoint1, imagebrush));
                square.Freeze();
                return square;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create square 2D
        /// </summary>
        /// <param name="pictureName"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public Model3DGroup CreateSquare2D(Color modelColor, Point3D[] points)
        {
            try
            {
                SolidColorBrush modelbrush = new SolidColorBrush(modelColor);
                modelbrush.Freeze();

                Model3DGroup square = new Model3DGroup();
                square.Children.Add(CreateGeoModel2D(points[0], points[3], points[1], modelbrush));
                square.Children.Add(CreateGeoModel2D(points[1], points[3], points[2], modelbrush));
                square.Freeze();
                return square;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create square 2D
        /// </summary>
        /// <param name="pictureName"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public Model3DGroup CreateSquare2D(string pictureName, Point3D[] points)
        {
            try
            {
                Uri inpuri = new Uri(@pictureName, UriKind.Relative);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = inpuri;
                bi.EndInit();
                ImageBrush imagebrush = new ImageBrush(bi);
                imagebrush.Opacity = 100;
                imagebrush.Freeze();

                Point[] ptexture0 = { new Point(0, 0), new Point(0, 1), new Point(1, 0) };
                Point[] ptexture1 = { new Point(1, 0), new Point(0, 1), new Point(1, 1) };


                Model3DGroup square = new Model3DGroup();
                square.Children.Add(CreateGeoModel2D(points[0], points[3], points[1], ptexture0, imagebrush));
                square.Children.Add(CreateGeoModel2D(points[1], points[3], points[2], ptexture1, imagebrush));
                square.Freeze();
                return square;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
