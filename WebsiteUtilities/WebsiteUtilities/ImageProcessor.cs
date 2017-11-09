using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WebsiteUtilities {
    public static class ImageProcessor {
        /// <summary>
        /// Rotates an image by 90 degrees and saves it over the original.
        /// </summary>
        /// <param name="filename">The full path of the image file to be rotated.</param>
        /// <param name="left">If true, this will rotate the image to the left instead of to the right.</param>
        /// <returns>True if successful. False otherwise.</returns>
        public static bool Rotate90AndSave(string filename, bool left) {
            try {
                //Load the image file
                Image img = Image.FromFile(filename);
                ImageFormat imgfrmt = img.RawFormat;
                //Rotate the image 90 degrees to either side
                img.RotateFlip(left ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone);
                //Save the file back over itself
                img.Save(filename, imgfrmt);
                //Get rid of the object
                img.Dispose();

                return true;
            } catch {
                return false;
            }
        }

        /// <summary>
        /// Rotates an image by 180 degrees and saves it over the original.
        /// </summary>
        /// <param name="filename">The full path of the image file to be rotated.</param>
        /// <returns>True if successful. False otherwise.</returns>
        public static bool Rotate180AndSave(string filename) {
            try {
                //Load the image file
                Image img = Image.FromFile(filename);
                ImageFormat imgfrmt = img.RawFormat;
                //Rotate the image 180 degrees to either side
                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                //Save the file back over itself
                img.Save(filename, imgfrmt);
                //Get rid of the object
                img.Dispose();

                return true;
            } catch {
                return false;
            }
        }

        /// <summary>
        /// Rotates an image by 90 degrees.
        /// </summary>
        /// <param name="imageData">A byte array of the image data.</param>
        /// <param name="left">If true, this will rotate the image to the left instead of to the right.</param>
        /// <param name="newImage">An output parameter containing the rotated image.</param>
        /// <returns>True if successful. False otherwise.</returns>
        public static bool Rotate90AndSave(byte[] imageData, bool left, out byte[] newImage) {
            try {
                using (MemoryStream ms = new MemoryStream(imageData))
                using (MemoryStream outputMS = new MemoryStream())
                using (Image image = Image.FromStream(ms)) {
                    ImageFormat imgfrmt = image.RawFormat;
                    //Rotate the image 90 degrees to either side
                    image.RotateFlip(left ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone);
                    //Save to the output stream
                    image.Save(outputMS, imgfrmt);
                    //Set up the return byte array
                    newImage = outputMS.ToArray();
                }

                return true;
            } catch (Exception ex) {
                ErrorHandler.WriteLog("WebsiteUtilities.ImageProcessor.Rotate90AndSave", "Error rotating image.", ErrorHandler.ErrorEventID.General, ex);
                newImage = null;
                return false;
            }
        }

        /// <summary>
        /// Rotates an image by 180 degrees.
        /// </summary>
        /// <param name="imageData">A byte array of the image data.</param>
        /// /// <param name="newImage">An output parameter containing the rotated image.</param>
        /// <returns>True if successful. False otherwise.</returns>
        public static bool Rotate180AndSave(byte[] imageData, out byte[] newImage) {
            try {
                using (MemoryStream ms = new MemoryStream(imageData))
                using (MemoryStream outputMS = new MemoryStream())
                using (Image image = Image.FromStream(ms)) {
                    ImageFormat imgfrmt = image.RawFormat;
                    //Rotate the image 180 degrees to either side
                    image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    //Save to the output stream
                    image.Save(outputMS, imgfrmt);
                    //Set up the return byte array
                    newImage = outputMS.ToArray();
                }
                return true;
            } catch (Exception ex) {
                ErrorHandler.WriteLog("WebsiteUtilities.ImageProcessor.Rotate180AndSave", "Error rotating image.", ErrorHandler.ErrorEventID.General, ex);
                newImage = null;
                return false;
            }
        }


        /// <summary>
        /// Returns the content type string (ie. image/jpeg) for jpeg, gif, png and tiff files. Defaults to image/bmp for anything else.
        /// </summary>
        /// <param name="img">The image to check.</param>
        public static string GetContentType(Image img) {
            if (img != null) {
                //Guid = {b96b3cae-0728-11d3-9d7b-0000f81ef32e}
                if (img.RawFormat.Equals(ImageFormat.Jpeg)) {
                    return "image/jpeg";
                } else if (img.RawFormat.Equals(ImageFormat.Gif)) {
                    return "image/gif";
                } else if (img.RawFormat.Equals(ImageFormat.Png)) {
                    return "image/png";
                } else if (img.RawFormat.Equals(ImageFormat.Tiff)) {
                    return "image/tiff";
                }
            }
            return "image/bmp";
        }

        public static Image GetTiffImageFromFile(string filename) {
            Image temp = null;
            //*
            using (Stream st = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                byte[] b = new byte[st.Length];
                st.Read(b, 0, b.Length);
                MemoryStream ms = new MemoryStream(b);
                using (Image image = Image.FromStream(ms)) {

                    int compressionTagIndex = Array.IndexOf(image.PropertyIdList, 0x103);
                    PropertyItem compressionTag = image.PropertyItems[compressionTagIndex];
                    short type = BitConverter.ToInt16(compressionTag.Value, 0);
                }
            }
            //*/

            /*byte[] buffer = System.IO.File.ReadAllBytes(filename);

            // create a memory stream out of them
            var ms = new System.IO.MemoryStream(buffer);
            
            Tiff tiff = Tiff.ClientOpen("in-memory", "r", ms, new TiffStream());
            */
            /*

             using (Tiff input = Tiff.Open(filename, "r")) {
                int width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                int height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                int[] raster = new int[height * width];
                if (input.ReadRGBAImage(width, height, raster)) {
                    //System.Windows.Forms.MessageBox.Show("Could not read image");
                    using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppRgb555)) {
                        Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                        BitmapData bmpdata = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                        byte[] bits = new byte[bmpdata.Stride * bmpdata.Height];
                        for (int y = 0; y < bmp.Height; y++) {
                            int rasterOffset = y * bmp.Width;
                            int bitsOffset = (bmp.Height - y - 1) * bmpdata.Stride;

                            for (int x = 0; x < bmp.Width; x++) {
                                int rgba = raster[rasterOffset++];
                                bits[bitsOffset++] = (byte) ((rgba >> 16) & 0xff);
                                bits[bitsOffset++] = (byte) ((rgba >> 8) & 0xff);
                                bits[bitsOffset++] = (byte) (rgba & 0xff);
                            }
                        }
                        System.Runtime.InteropServices.Marshal.Copy(bits, 0, bmpdata.Scan0, bits.Length);
                        bmp.UnlockBits(bmpdata);

                        bmp.Save(@"c:\test.png", ImageFormat.Png);
                        //System.Diagnostics.Process.Start("c:\test.png");
                    }
                }
            }
            //*/

            return temp;
        }
    }
}

