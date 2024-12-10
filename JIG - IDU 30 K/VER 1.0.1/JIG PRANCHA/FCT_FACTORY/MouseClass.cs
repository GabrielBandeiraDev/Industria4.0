using System.Drawing;
using System.Windows.Forms;

namespace FCT_FACTORY
{
    class MouseClass
    {
        public int addNewSubPosition = 1;

        public void MoveCursor(string coord, Cursor cs)
        {
            //pos = pos - 1;
            //pos = pos * addNewSubPosition;
            string[] posXY = coord.Split(':');
            int x = int.Parse(posXY[0]);
            int y = int.Parse(posXY[1]);

            //y += pos;

            //MessageBox.Show(x.ToString() + "\r\n" + y.ToString() + "\r\n" + addNewSubPosition);

            cs = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(x, y);
        }


        public void pasteMsg()
        {
            SendKeys.Send("^v");
        }

        public void pressEnter()
        {
            //  KeyboardSimulator.KeyPress(Keys.Enter);
        }


        public void dobleClick()
        {

        }

        public void clickLeft()
        {


        }

        public string writeCoordinates(int x, int y, int sx, int sy)
        {
            string msg = x.ToString() + ":" + y.ToString() + ":" + sx.ToString() + ":" + sy.ToString();
            return msg;
        }


    }
}
