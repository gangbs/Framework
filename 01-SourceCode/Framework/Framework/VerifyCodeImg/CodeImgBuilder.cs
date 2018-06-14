using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class CodeImgBuilder
    {
        Bitmap bmp;
        Graphics graph;
        Random r;
        readonly string code;
        readonly int height;
        readonly int width;
        readonly Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };//写死
        //readonly Color backgroundColor;

        public CodeImgBuilder(string code, int h, int w)
        {
            this.code = code;
            this.height = h;
            this.width = w;
            r = new Random();
        }

        //创建画布
        void BuildCanvas()
        {
            bmp = new Bitmap(width, height);
            graph = Graphics.FromImage(bmp);
            graph.Clear(Color.White);//写死
        }

        //画噪线
        void BuildNoiseLine()
        {
            //画噪线 
            for (int i = 0; i < code.Length; i++)
            {
                int x1 = r.Next(width);
                int y1 = r.Next(height);
                int x2 = r.Next(width);
                int y2 = r.Next(height);
                Color clr = color[r.Next(color.Length)];
                graph.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
        }

        //画验证码字符串
        void BuildCode()
        {
            for (int i = 0; i < code.Length; i++)
            {
                Font font = new Font("Times New Roman", 16);//写死
                Color clr = color[r.Next(color.Length)];
                graph.DrawString(code[i].ToString(), font, new SolidBrush(clr), (float)i * 18, (float)0);//写死了
            }
        }

        //构建
        public void Construct()
        {
            this.BuildCanvas();
            this.BuildNoiseLine();
            this.BuildCode();
        }

        //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 
        public byte[] GetImgByte()
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                graph.Dispose();
                bmp.Dispose();
            }
        }
    }
}
