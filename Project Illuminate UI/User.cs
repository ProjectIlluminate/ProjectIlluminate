using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Illuminate_UI
{
    class User//User profile
    {
        //public int ID { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }//height in cms

        //int id,
        public User(string name, int height)
        {
            //ID = id;
            Name = name;
            Height = height;
        }


        public override string ToString()
        {
            return string.Format("Name: {0}\n Height: {1}\n", Name, Height);
        }

        public string FileWriteFormat()
        {
            return Name + "," + Height;
        }
    }//end class User
}
