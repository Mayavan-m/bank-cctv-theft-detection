using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoSource;
using Tiger.Video.VFW;

using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Drawing.Imaging;
namespace MotionDetection
{
    public partial class LoadCamera : Form
    {

       private const int statLength = 15;
       private int statIndex = 0, statReady = 0;
       private int[] statCount = new int[statLength];

       private IMotionDetector detector = new MotionDetector3Optimized();
       private int detectorType = 4;
       private int intervalsToSave = 0;

       private AVIWriter writer = null;
       private bool saveOnMotion = false;




       public string dename="";




       public LoadCamera()
       {
           InitializeComponent();
       }
       CaptureDeviceForm form;
       private void button1_Click(object sender, EventArgs e)
       {


            form = new CaptureDeviceForm();

           if (form.ShowDialog(this) == DialogResult.OK)
           {
               // create video source
               CaptureDevice localSource = new CaptureDevice();
               localSource.VideoSource = form.Device;
               dename = form.Device;
               // open it
               OpenVideoSource(localSource);


           }


           detector = new MotionDetector1();
           detectorType = 1;
           SetMotionDetector();
       }



       private void SetMotionDetector()
       {
           Camera camera = cameraWindow.Camera;

           // enable/disable motion alarm
    if (detector != null)
    {
        detector.MotionLevelCalculation = motionAlarmItem.Checked;
    }

    // set motion detector to camera
    if (camera != null)
    {
        camera.Lock();
        camera.MotionDetector = detector;

        // reset statistics
        statIndex = statReady = 0;
        camera.Unlock();
    }
}
private void OpenVideoSource(IVideoSource source)
{
    // set busy cursor
    this.Cursor = Cursors.WaitCursor;

    // close previous file
    CloseFile();

    // enable/disable motion alarm
    if (detector != null)
    {
        detector.MotionLevelCalculation = motionAlarmItem.Checked;

    }


    // create camera
    Camera camera = new Camera(source, detector);
    // start camera
    camera.Start();

    // attach camera to camera window
    cameraWindow.Camera = camera;

    // reset statistics
    statIndex = statReady = 0;

    // set event handlers
    camera.NewFrame += new EventHandler(camera_NewFrame);
    camera.Alarm += new EventHandler(camera_Alarm);

    // start timer
    timer.Start();

    this.Cursor = Cursors.Default;
}

// Close current file
private void CloseFile()
{
    Camera camera = cameraWindow.Camera;

    if (camera != null)
    {
        // detach camera from camera window
        cameraWindow.Camera = null;
         // signal camera to stop
         camera.SignalToStop();
         // wait for the camera
         camera.WaitForStop();

         camera = null;

         if (detector != null)
             detector.Reset();
    }

    if (writer != null)
    {
        writer.Dispose();
        writer = null;
    }
    intervalsToSave = 0;




}

private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
{
    Camera camera = cameraWindow.Camera;

    if (camera != null)
    {
        // get number of frames for the last second
        statCount[statIndex] = camera.FramesReceived;

         // increment indexes
         if (++statIndex >= statLength)
             statIndex = 0;
         if (statReady < statLength)
             statReady++;

         float fps = 0;

         // calculate average value
         for (int i = 0; i < statReady; i++)
         {
             fps += statCount[i];
         }
         fps /= statReady;

         statCount[statIndex] = 0;

        // label1.Text = fps.ToString("F2") + " fps";
    }

    // descrease save counter
    if (intervalsToSave > 0)
    {
        if ((--intervalsToSave == 0) && (writer != null))
        {
            writer.Dispose();
            writer = null;
        }
           }
       }


       // On alarm
       private void camera_Alarm(object sender, System.EventArgs e)
       {
           // save movie for 5 seconds after motion stops
           intervalsToSave = (int)(5 * (1000 / timer.Interval));



       }

       // On new frame
       private void camera_NewFrame(object sender, System.EventArgs e)
       {
           if ((intervalsToSave != 0) && (saveOnMotion == true))
           {


                 this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate()
                 {




                  //     Face2 ff = new Face2();

                    string Compress =
System.IO.Path.GetDirectoryName(Application.ExecutablePath.ToString()) + "\\123.wav";
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer();

                       player.SoundLocation = Compress;
                       player.Play();
                       Login ff = new Login();

                       ff.devicename = dename;
                       ff.Show();
                       CloseFile();
                       this.Close();

                 });




                 //Camera camera = cameraWindow.Camera;

                 //camera.Lock();

               // this.Close();


            //    camera.Unlock();




           }
       }
       public void alert()
       {


           //two mail id or more
           string to = "receiver@example.com";


           //your mail id
           string from = "sender@example.com";
           // string subject = "Key";
           // string body = TextBox1.Text;

            //your password
            string password = "YOUR_APP_PASSWORD"; // placeholder
            using (MailMessage mm = new MailMessage(from, to))
            {
                mm.Subject = "Alert";
                mm.Body = "Motion Detection";
                //if (fuAttachment.HasFile)
                //{
                //    string FileName =
Path.GetFileName(fuAttachment.PostedFile.FileName);
                //    mm.Attachments.Add(new
Attachment(fuAttachment.PostedFile.InputStream, FileName));
                //}
                Camera camera = cameraWindow.Camera;
                Bitmap image = camera.LastFrame;

               System.IO.MemoryStream stream = new System.IO.MemoryStream();
               image.Save(stream, ImageFormat.Jpeg);
               stream.Position = 0;

               mm.Attachments.Add(new Attachment(stream, "Screenshot.jpg"));

               mm.IsBodyHtml = false;
               SmtpClient smtp = new SmtpClient();
               smtp.Host = "smtp.gmail.com";
               smtp.EnableSsl = true;
               NetworkCredential NetworkCred = new NetworkCredential(from, password);
               smtp.UseDefaultCredentials = true;
               smtp.Credentials = NetworkCred;
               smtp.Port = 587;
               smtp.Send(mm);
               //MessageBox.Show("Mail Send!");

           }




       }

       private void motionAlarmItem_CheckedChanged(object sender, EventArgs e)
       {
           detector = new MotionDetector1();
           detectorType = 1;
             SetMotionDetector();

             // change saving mode
             saveOnMotion = !saveOnMotion;

             // update menu
             detectorSaveItem.Checked = saveOnMotion;


        //     grabber = new Capture();
             //Application.Idle += new EventHandler(FrameGrabber_Standard);

       }

       private void LoadCamera_Load(object sender, EventArgs e)
       {


             if (System.DateTime.Now.Hour >= 15)
             {
                 motionAlarmItem.Checked = true;



             }
             else
             {
                 motionAlarmItem.Checked = false;
             }



             if (dename == "")
             {
             }
             else
             {
                 CaptureDevice localSource = new CaptureDevice();
                 localSource.VideoSource = dename;

                 // open it
                 OpenVideoSource(localSource);


                 detector = new MotionDetector1();
                 detectorType = 1;
                 SetMotionDetector();

                 timer1.Start();

             }

       }

       public void sendmessage(string targetno, string message)
       {

            String query =
"https://YOUR_SMS_PROVIDER/api?username=YOUR_USER&password=YOUR_PASSWORD&to=" + targetno + "&message=" + message;
            WebClient client = new WebClient();
            Stream sin = client.OpenRead(query);
            // Response.Write("<script> alert('Message Send') </script>");
        }


        int ttt = 0;


        public string mm;

        private void timer1_Tick(object sender, EventArgs e)
        {


            ttt++;

            if (ttt == 100)
            {
                ttt = 0;
                timer.Stop();
                motionAlarmItem.Checked = true;


                if (mm == "in")
                {

                     //   sendmessage("PHONE_NUMBER", "Anonymous Detection");
                }
            }


        }




    }
}
