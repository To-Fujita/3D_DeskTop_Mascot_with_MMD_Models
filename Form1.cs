// Desk Top Mascot by T. Fujita on 2021/01/03

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Timers;

using DxLibDLL;

namespace DeskTop_Mascot
{
    public partial class Form1 : Form
    {
        private int physicsMode;                                        // Physics calculation mode
        public static int modelNo = 0;                                  // Model No. for display
        public static int modelHandle;                                  // Model handle for DX Lib
        public static int attachIndex;                                  // Motion No. for DX Lib
        public static int MotionMode = 0;                               // 0: Display in order, 1: Loop, -1: One-shot
        public static int MotionNo = 0;                                    // Motion No. for display
        public static int MaxModels = 1;                                // Max Models count
        public static int MaxMotions = 1;                               // Max Motions count
        public static int MaxSchedule = 1;                              // Max Schedule count
        public static int DSP_flag = 0;                                 // Flag for display in TopMost (0: Normal, 1: TopMost)
        public static int DSP_change_flag = 0;                          // Flag for display mode change (0: No change, 1: Change)
        public static int Phy_flag = 1;                                 // Flag for physics calc. (0: No calculation, -1: Realtime calculation, 1: calculation at loading）
        public static int Phy_change_flag = 0;                          // Flag for physics change (0: No change, 1: Change)
        public static int WinPos_X = 200;                               // Position X for information window
        public static int WinPos_Y = 200;                               // Position Y for information window
        public static int Schedule_flag = 1;                            // 0: Schedule OFF, 1: Schedule ON
        public static int OneShot_flag = 0;                             // 0: Before Working, 1: After Working
        public static float totalTime;                                  // Total time for the motion
        public static float Camera_X = 0.0f;                            // Camera Position X
        public static float Camera_Y = 10.0f;                           // Camera Position Y
        public static float Camera_Z = -30.0f;                          // Camera Position Z
        public static float playSpeed = 0.6f;                           // Play speed for Motions
        public static float normalSpeed = 0.6f;                         // Normal Speed for Motions
        public static float MaxSpeed = 3.0f;                            // Max Speed for Motions
        public static float playTime;                                   // Play time for motion
        public static string Model;                                     // Model for display
        public static string[] models_url = new string[MaxModels];      // Folder name + file name for models
        public static string[] models_name = new string[MaxModels];     // Model name
        public static string[] motions_no = new string[MaxMotions];     // Motion No.
        public static string[] motions_name = new string[MaxMotions];   // Motion name
        public static string[] schedule_SH = new string[MaxSchedule];   // Start hour for schedule
        public static string[] schedule_SM = new string[MaxSchedule];   // Start minutes for schedule
        public static string[] schedule_MN = new string[MaxSchedule];   // Motion No. for schedule
        public static string[] schedule_SP = new string[MaxSchedule];   // Motion speed for schedule
        public static string[] schedule_PH = new string[MaxSchedule];   // Physics Calc. ON/OFF
        public static string[] schedule_CM = new string[MaxSchedule];   // Command for schedule
        public static string[] schedule_AD1 = new string[MaxSchedule];  // Additional command No.1 for schedule
        public static string[] schedule_AD2 = new string[MaxSchedule];  // Additional command No.2 for schedule

        public static int Temp;
        public static int Mouse_X;                                      // for drag with mouse the model
        public static int Mouse_Y;
        public static int Obj_X;
        public static int Obj_Y;
        public static int Dist_X;
        public static int Dist_Y;
        public static int Mouse_flag = 0;
        public static int Mouse_OrgX;
        public static int Mouse_OrgY;
        public static float Obj_OrgX;
        public static float Obj_OrgY;
        public static float Obj_OrgZ;
        string Temp_Command;
        DateTime dt;
        
        public Form1()
        {
            dt = DateTime.Now;
            models_url[0] = "Data/AyaseHina160/AyaseHina.mv1";
            models_name[0] = "Ayase Hina";
            motions_no[0] = "0";
            motions_name[0] = "To bow";

            CSV_Read CSV_Load = new CSV_Read();
            try
            {
                CSV_Load.Read_CSV();
            }
            catch (Exception)
            {
                CSV_Save CSV_Write = new CSV_Save();
                CSV_Write.Save_CSV();
            }

            Random rnd = new System.Random();
            modelNo = rnd.Next(0, (MaxModels));
            Model = models_url[modelNo];

            InitializeComponent();                          // Initialize the form

            ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);   // Set the screen size
            Text = "DesctopMascot";                         // Window's name

            DX.SetOutApplicationLogValidFlag(DX.FALSE);     // Do not create the Log.txt
            DX.SetUserWindow(Handle);                       // Set this Form as a parent window for DxLib

            DX.SetZBufferBitDepth(24);                      // Set 24Bit for z-buffer in DxLib
            DX.SetCreateDrawValidGraphZBufferBitDepth(24);  // Set 24Bit for z-buffer for back screen in DxLib
            DX.SetFullSceneAntiAliasingMode(4, 2);          // Set the anti alias mode for full screen

            DX.DxLib_Init();                                // Initialize the DxLib
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);            // Set the back screen for drawing
            physicsMode = DX.MV1SetLoadModelUsePhysicsMode(DX.DX_LOADMODEL_PHYSICS_LOADCALC);

            modelHandle = DX.MV1LoadModel(Model);           // Read MMD Model
            MaxMotions = DX.MV1GetAnimNum(modelHandle);     // Read Max Animations
            attachIndex = DX.MV1AttachAnim(modelHandle, MotionNo, -1, DX.FALSE);    // Set the motion No.
            totalTime = DX.MV1GetAttachAnimTotalTime(modelHandle, attachIndex);     // Get the total time for motion
            playTime = 0.0f;

            DX.SetCameraNearFar(0.1f, 1000.0f);             // Set the drawing range for camera
            DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(Camera_X, Camera_Y, Camera_Z), DX.VGet(0.0f, 10.0f, 0.0f)); // Set the direction for camera

        }

        public void MainLoop()
        {
            System.Timers.Timer timer = new System.Timers.Timer(60000);
            timer.Elapsed += (sender, e) =>
            {
                if (Schedule_flag == 1)
                {
                    dt = DateTime.Now;
                    int Temp_SH = dt.Hour;
                    int Temp_SM = dt.Minute;
                    int Temp_SS = dt.Second;
                    int Rand_Count = 0;
                    int Rand_Flag = 0;
                    string[] Temp_Motion = new string[0];
                    string[] Temp_Speed = new string[0];
                    string[] Temp_Phy = new string[0];
                    string[] Temp_CM = new string[0];
                    string[] Temp_AD1 = new string[0];
                    string[] Temp_AD2 = new string[0];
                    Temp = 0;

                    if (Temp_SS < 1)
                    {
                        for (int i = 0; i < schedule_SH.Length; i++)
                        {
                            if (Temp_SH == int.Parse(schedule_SH[i]) && Temp_SM == int.Parse(schedule_SM[i]))
                            {
                                Temp_Command = schedule_AD1[i];
                                MotionNo = int.Parse(schedule_MN[i]);
                                playSpeed = float.Parse(schedule_SP[i]);
                                MotionMode = -1;
                                if (schedule_PH[i] == "ON" && Phy_flag == 0)
                                {
                                    Phy_change_flag = 1;
                                    Phy_flag = -1;
                                }
                                else if (schedule_PH[i] == "OFF" && Phy_flag != 0)
                                {
                                    Phy_change_flag = 1;
                                    Phy_flag = 0;
                                }

                                if (schedule_CM[i] == "Loop")
                                {
                                    MotionMode = 1;
                                }
                                if (schedule_CM[i] == "One-Shot")
                                {
                                    MotionMode = -1;
                                    OneShot_flag = 0;
                                }

                                if (Temp_Command == "Random")
                                {
                                    Rand_Flag = 1;
                                    if (Rand_Count >= Temp_Motion.Length)
                                    {
                                        Array.Resize(ref Temp_Speed, Temp_Speed.Length + 1);
                                        Array.Resize(ref Temp_Phy, Temp_Phy.Length + 1);
                                        Array.Resize(ref Temp_Motion, Temp_Motion.Length + 1);
                                        Array.Resize(ref Temp_CM, Temp_CM.Length + 1);
                                    }
                                    Temp_Speed[Rand_Count] = schedule_SP[i];
                                    Temp_Phy[Rand_Count] = schedule_PH[i];
                                    Temp_Motion[Rand_Count] = schedule_MN[i];
                                    Temp_CM[Rand_Count] = schedule_CM[i];
                                    Rand_Count = Rand_Count + 1;
                                }
                            }
                        }
                        if (Rand_Flag == 1)
                        {
                            Random r1 = new System.Random();
                            Temp = r1.Next(0, Rand_Count);
                            MotionNo = int.Parse(Temp_Motion[Temp]);
                            playSpeed = float.Parse(Temp_Speed[Temp]);
                            if (Temp_Phy[Temp] == "ON" && Phy_flag == 0)
                            {
                                Phy_change_flag = 1;
                                Phy_flag = -1;
                            }
                            else if (Temp_Phy[Temp] == "OFF" && Phy_flag != 0)
                            {
                                Phy_change_flag = 1;
                                Phy_flag = 0;
                            }
                            MotionMode = -1;
                            if (Temp_CM[Temp] == "Loop")
                            {
                                MotionMode = 1;
                            }
                            if(Temp_CM[Temp] == "One-Shot")
                            {
                                MotionMode = -1;
                                OneShot_flag = 0;
                            }
                        }
                    }
                }
            };
            timer.Start();

            if (playSpeed < 0.0f) { playSpeed = 0.0f; }
            if (playSpeed > MaxSpeed) { playSpeed = MaxSpeed; }
            if (MotionNo < 0) { MotionNo = 0; }
            if (MotionNo > MaxMotions) { MotionNo = 0; }

            float Distance = (float)Math.Sqrt(Camera_X * Camera_X + Camera_Y * Camera_Y + Camera_Z * Camera_Z);
            DX.ClearDrawScreen();                                       // Clear the back screen
            DX.DrawBox(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, DX.GetColor(1, 1, 1), DX.TRUE); // Set the transparent
            playTime += playSpeed;                                      // Set the next frame for the motion
 
            if (playTime >= totalTime)
            {
                DX.MV1DetachAnim(modelHandle, attachIndex);             // Reset the motion
                playTime = 0.0f;
                if (MotionMode == -1 && OneShot_flag > 0)
                {
                    playTime = totalTime;
                }
                if (MotionMode == -1 && OneShot_flag == 0)
                {
                    OneShot_flag = 1;
                }
                if (MotionMode == 0)
                {
                    MotionNo = MotionNo + 1;
                }
                if (MotionNo > MaxMotions)
                {
                    MotionNo = 0;
                }
                attachIndex = DX.MV1AttachAnim(modelHandle, MotionNo, -1, DX.FALSE);
                totalTime = DX.MV1GetAttachAnimTotalTime(modelHandle, attachIndex);
                physicsMode = DX.MV1PhysicsResetState(modelHandle);
            }

            DX.MV1SetAttachAnimTime(modelHandle, attachIndex, playTime);    // Set the display frame for motion
            if (Phy_flag == -1)
            {
                DX.MV1PhysicsCalculation(modelHandle, 1000.0f / 60.0f);     // Physics calculation
            }
            DX.MV1DrawModel(modelHandle);                                   // Draw the motion

            // Closing at push the ESC key
            if (DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) != 0)
            {
                DX.DxLib_End();
                Close();
            }

            DX.ScreenFlip();

            if (DSP_change_flag != 0)
            {
                if (DSP_flag == 0)
                {
                    this.TopMost = false;
                }
                if (DSP_flag == 1)
                {
                    this.TopMost = true;
                }
                DSP_change_flag = 0;
            }

            if (Phy_change_flag != 0)
            {
                if (Phy_flag == -1)
                {
                    DX.MV1PhysicsResetState(modelHandle);
                    DX.MV1SetLoadModelPhysicsCalcPrecision(0);      // Calculation accuracy  0: Default(60FPS), 1: 120FPS, 2: 240FPS, 3: 480FPS, 4: 960FPS, 5: 1920FPS  
                    physicsMode = DX.MV1SetLoadModelUsePhysicsMode(DX.DX_LOADMODEL_PHYSICS_REALTIME);
                }
                if (Phy_flag == 1)
                {
                    DX.MV1PhysicsResetState(modelHandle);
                    DX.MV1SetLoadModelPhysicsCalcPrecision(0);
                    physicsMode = DX.MV1SetLoadModelUsePhysicsMode(DX.DX_LOADMODEL_PHYSICS_LOADCALC);
                }
                if (Phy_flag == 0)
                {
                    DX.MV1PhysicsResetState(modelHandle);
                    physicsMode = DX.MV1SetLoadModelUsePhysicsMode(DX.DX_LOADMODEL_PHYSICS_DISABLE);
                }
                modelHandle = DX.MV1LoadModel(Model);
                MaxMotions = DX.MV1GetAnimNum(modelHandle);
                attachIndex = DX.MV1AttachAnim(modelHandle, MotionNo, -1, DX.FALSE);
                totalTime = DX.MV1GetAttachAnimTotalTime(modelHandle, attachIndex);
                Phy_change_flag = 0;
            }

            if ((DX.GetMouseInput() & DX.MOUSE_INPUT_LEFT) != 0)
            {
                Temp = DX.GetMousePoint(out Mouse_X, out Mouse_Y);
                DX.VECTOR pos = DX.ConvWorldPosToScreenPos(DX.MV1GetPosition(modelHandle));
                Obj_X = (int)pos.x;
                Obj_Y = (int)pos.y;

                if ((Math.Abs(Mouse_X - Obj_X) < (50 * 30 / Distance)) && ((Mouse_Y - Obj_Y) < (-100 * 30 / Distance) && (Mouse_Y - Obj_Y) > (-700 * 30 / Distance)))
                {
                    if (Mouse_flag == 0)
                    {
                        DX.VECTOR ObjPos = DX.MV1GetPosition(modelHandle);
                        Obj_OrgX = ObjPos.x;
                        Obj_OrgY = ObjPos.y;
                        Obj_OrgZ = ObjPos.z;
                        Mouse_OrgX = Mouse_X;
                        Mouse_OrgY = Mouse_Y;
                        Mouse_flag = 1;
                    }
                    Dist_X = Mouse_X - Mouse_OrgX;
                    Dist_Y = Mouse_Y - Mouse_OrgY;
                    DX.VECTOR Move_pos = DX.VGet(Obj_OrgX + (float)Dist_X * (Distance + 0.1f) / 1000.0f, Obj_OrgY - (float)Dist_Y * (Distance + 0.1f) / 1000.0f, Obj_OrgZ);
                    DX.MV1SetPosition(modelHandle, Move_pos);
                }
            }
            else
            {
                Mouse_flag = 0;
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DX.DxLib_End();
            Application.Exit();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            TransparencyKey = Color.FromArgb(1, 1, 1);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DX.DxLib_End();
            Application.Exit();
        }

        private void cameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_Camera dialog1 = new Dialog_Camera();
            dialog1.Show();             // Show the dialog window for setting camera works
        }

        private void motionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_Motions dialog1 = new Dialog_Motions();
            dialog1.Show();             // Show the dialog window to select the motion
        }

        private void modelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_Models dialog1 = new Dialog_Models();
            dialog1.Show();             // Show the dialog window to selkect the model
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position.X - 10, Cursor.Position.Y - 10);
        }

        private void topMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_DisplayMode dialog1 = new Dialog_DisplayMode();
            dialog1.Show();             // Show dialog window to set the top most display
        }

        private void physicsCalcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_PhysicsMode dialog1 = new Dialog_PhysicsMode();
            dialog1.Show();             // Show dialog window to set the physics calc. mode
        }

        private void settingWindowPosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_WindowPos dialog1 = new Dialog_WindowPos();
            DialogResult result = dialog1.ShowDialog();         // Show dialog window to set the position for the dialog window
            if (result == DialogResult.OK)
            {
                WinPos_X = int.Parse(dialog1.textBox_X.Text);
                WinPos_Y = int.Parse(dialog1.textBox_Y.Text);
            }
        }

        private void scheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_Schedule dialog1 = new Dialog_Schedule();
            dialog1.Show();             // Show dialog window to set the motion speed
        }
    }

    // Dialog Windows
    public class Dialog_Camera : Form           // Camera Control
    {
        Label label_CX;
        Label label_CY;
        Label label_CZ;
        TextBox textBox_CX;
        TextBox textBox_CY;
        TextBox textBox_CZ;
        TrackBar trackbar_CX;
        TrackBar trackbar_CY;
        TrackBar trackbar_CZ;
        Button Close_button;
        int val_CX = (int)Form1.Camera_X;
        int val_CY = (int)Form1.Camera_Y;
        int val_CZ = (int)Form1.Camera_Z;

        public Dialog_Camera()
        {
            // Set the dialog window
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = true;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(300, 400);
            this.Location = new Point(Form1.WinPos_X, Form1.WinPos_Y);
            this.Text = "Set the Camera Position";

            label_CX = new Label()
            {
                Text = "X Position: ",
                Location = new Point(10, 10),
            };
            textBox_CX = new TextBox()
            {
                Text = val_CX.ToString(),
                Location = new Point(150, 10),
            };
            trackbar_CX = new TrackBar()
            {
                Location = new Point(20, 30),
                Minimum = -100,
                Maximum = 100,
                Value = val_CX,
                TickFrequency = 10,
                SmallChange = 1,
                LargeChange = 10,
                AutoSize = false,
                Size = new Size(240, 40),
            };
            trackbar_CX.ValueChanged += Trackbar_CX_ValueChanged;

            label_CY = new Label()
            {
                Text = "Y Position: ",
                Location = new Point(10, 100),
            };
            textBox_CY = new TextBox()
            {
                Text = val_CY.ToString(),
                Location = new Point(150, 100),
            };
            trackbar_CY = new TrackBar()
            {
                Location = new Point(20, 120),
                Minimum = -100,
                Maximum = 100,
                Value = val_CY,
                TickFrequency = 10,
                SmallChange = 1,
                LargeChange = 10,
                AutoSize = false,
                Size = new Size(240, 40),
            };
            trackbar_CY.ValueChanged += Trackbar_CY_ValueChanged;

            label_CZ = new Label()
            {
                Text = "Z Position: ",
                Location = new Point(10, 200),
            };
            textBox_CZ = new TextBox()
            {
                Text = val_CZ.ToString(),
                Location = new Point(150, 200),
            };
            trackbar_CZ = new TrackBar()
            {
                Location = new Point(20, 220),
                Minimum = -100,
                Maximum = 100,
                Value = val_CZ,
                TickFrequency = 10,
                SmallChange = 1,
                LargeChange = 10,
                AutoSize = false,
                Size = new Size(240, 40),
            };
            trackbar_CZ.ValueChanged += Trackbar_CZ_ValueChanged;

            Close_button = new Button()
            {
                Text = "Close",
                Location = new Point(100, 300),
            };
            Close_button.Click += new EventHandler(Close_button_CLicked);

            this.Controls.AddRange(new Control[] {
                label_CX, label_CY, label_CZ, textBox_CX, trackbar_CX,
                textBox_CY, trackbar_CY, textBox_CZ, trackbar_CZ, Close_button
            });
        }

        void Close_button_CLicked(object sender, EventArgs e)
        {
            this.Close();
        }

        void Trackbar_CX_ValueChanged(object s, EventArgs e)
        {
            textBox_CX.Text = trackbar_CX.Value.ToString();
            Form1.Camera_X = (float)int.Parse(textBox_CX.Text);
            DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(Form1.Camera_X, Form1.Camera_Y, Form1.Camera_Z), DX.VGet(0.0f, 10.0f, 0.0f));
        }

        void Trackbar_CY_ValueChanged(object s, EventArgs e)
        {
            textBox_CY.Text = trackbar_CY.Value.ToString();
            Form1.Camera_Y = (float)int.Parse(textBox_CY.Text);
            DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(Form1.Camera_X, Form1.Camera_Y, Form1.Camera_Z), DX.VGet(0.0f, 10.0f, 0.0f));
        }

        void Trackbar_CZ_ValueChanged(object s, EventArgs e)
        {
            textBox_CZ.Text = trackbar_CZ.Value.ToString();
            Form1.Camera_Z = (float)int.Parse(textBox_CZ.Text);
            DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(Form1.Camera_X, Form1.Camera_Y, Form1.Camera_Z), DX.VGet(0.0f, 10.0f, 0.0f));
        }
    }

    public class Dialog_Motions : Form      // Select the Motion
    {
        Label label;
        Label label_1;
        Label label_SP;
        ListBox listBox;
        Button Serial_button;
        Button Select_button;
        Button close_button;
        TrackBar trackbar_SP;
        TextBox textBox_SP;
        string Temp_Text_1;
        int val_SP = (int)(Form1.playSpeed / Form1.normalSpeed * 100.0f);

        public Dialog_Motions()
        {
            this.MaximizeBox = false; 
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(300, 420);
            this.Location = new Point(Form1.WinPos_X, Form1.WinPos_Y);  
            this.Text = "Motion Select";

            label = new Label()
            {
                Location = new Point(20, 10),
                Text = "Select the Motion",
                Size = new Size(200, 24),
            };
            listBox = new ListBox()
            {
                Location = new Point(30, 40),
                Size = new Size(200, 80),
            };
            for (int i = 0; i < Form1.motions_name.Length; i++)
            {
                listBox.Items.Add(i + ": " + Form1.motions_name[i]);
            }
            listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;

            Serial_button = new Button()
            {
                Text = "In order",
                Location = new Point(30, 150),
            };
            Serial_button.Click += new EventHandler(Serial_button_CLicked);

            Select_button = new Button()
            {
                Text = "Loop",
                Location = new Point(150, 150),
            };
            Select_button.Click += new EventHandler(Select_button_CLicked);
            if (Form1.MotionMode == 0)
            {
                Temp_Text_1 = "Selected: In order";
            }
            if (Form1.MotionMode == 1)
            {
                Temp_Text_1 = "Selected: Loop";
            }
            label_1 = new Label()
            {
                Location = new Point(20, 180),
                Text = Temp_Text_1,
                Size = new Size(200, 24),
            };

            label_SP = new Label()
            {
                Text = "Animation Speed(%): ",
                Location = new Point(20, 240),
                Size = new Size(120, 24),
            };
            textBox_SP = new TextBox()
            {
                Text = val_SP.ToString(),
                Location = new Point(150, 240),
            };
            trackbar_SP = new TrackBar()
            {
                Location = new Point(20, 270),
                Minimum = 0,
                Maximum = 300,
                Value = val_SP,
                TickFrequency = 10,
                SmallChange = 1,
                LargeChange = 10,
                AutoSize = false,
                Size = new Size(240, 40),
            };
            trackbar_SP.ValueChanged += Trackbar_SP_ValueChanged;

            close_button = new Button()
            {
                Text = "Close",
                Location = new Point(150, 330),
            };
            close_button.Click += new EventHandler(Close_button_CLicked);

            this.Controls.AddRange(new Control[]
            {
                label, label_1, listBox, Serial_button, Select_button, label_SP, trackbar_SP, trackbar_SP, textBox_SP, close_button
            });
        }

        void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DX.MV1DetachAnim(Form1.modelHandle, Form1.attachIndex);
            Form1.MotionNo = listBox.SelectedIndex;
            Form1.attachIndex = DX.MV1AttachAnim(Form1.modelHandle, Form1.MotionNo, -1, DX.FALSE);   // Set the Motion
            Form1.totalTime = DX.MV1GetAttachAnimTotalTime(Form1.modelHandle, Form1.attachIndex); // Get total time of the motion
            Form1.playTime = 0.0f;
        }

        void Serial_button_CLicked(object sender, EventArgs e)
        {
            label_1.Text = "Selected: In order";
            Form1.MotionMode = 0;
        }

        void Select_button_CLicked(object sender, EventArgs e)
        {
            label_1.Text = "Selected: Loop";
            Form1.MotionMode = 1;
        }

         void Trackbar_SP_ValueChanged(object s, EventArgs e)
        {
            textBox_SP.Text = trackbar_SP.Value.ToString();
            Form1.playSpeed = (float)(float.Parse(textBox_SP.Text) * Form1.normalSpeed / 100.0f);
        }

        void Close_button_CLicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class Dialog_Models : Form      // Select the Model
    {
        Label label;
        ListBox listBox;
        Button close_button;
        Label label_Sel;

        public Dialog_Models()
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(300, 300);
            this.Location = new Point(Form1.WinPos_X, Form1.WinPos_Y);
            this.Text = "Model Select";

            label = new Label()
            {
                Location = new Point(20, 20),
                Text = "Select the Model",
                Size = new Size(200, 24),
            };
            listBox = new ListBox()
            {
                Location = new Point(30, 50),
                Size = new Size(200, 80),
            };
            for (int i = 0; i < Form1.models_url.Length; i++)
            {
                listBox.Items.Add(i + ": " + Form1.models_name[i]);
            }
            listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;

            label_Sel = new Label()
            {
                Text = "Selected Model: " + Form1.models_name[Form1.modelNo],
                Location = new Point(30, 150),
                Size = new Size(200, 25),
            };

            close_button = new Button()
            {
                Text = "Close",
                Location = new Point(150, 200),
            };
            close_button.Click += new EventHandler(Close_button_CLicked);

            this.Controls.AddRange(new Control[]
            {
                label, label_Sel, listBox, close_button
            });
        }

        void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex >= 0 && listBox.SelectedIndex <= Form1.MaxModels)
            {
                Form1.modelNo = listBox.SelectedIndex;
                Form1.Model = Form1.models_url[listBox.SelectedIndex];
                Form1.modelHandle = DX.MV1LoadModel(Form1.Model);                                       // Set the Model
                Form1.MaxMotions = DX.MV1GetAnimNum(Form1.modelHandle);                                 // Read Max Animations
                label_Sel.Text = "Selected Model: " + Form1.models_name[Form1.modelNo];
                Form1.attachIndex = DX.MV1AttachAnim(Form1.modelHandle, Form1.MotionNo, -1, DX.FALSE);  // Set the Motion
                Form1.totalTime = DX.MV1GetAttachAnimTotalTime(Form1.modelHandle, Form1.attachIndex);   // Get total time of the motion
            }
        }

        void Close_button_CLicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class Dialog_DisplayMode : Form      // Select the Display Mode
    {
        Label label;
        Label label_Sel;
        Button button_A;
        Button button_B;
        Button close_button;
        public string Temp_text;

        public Dialog_DisplayMode()
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(300, 300);
            this.Location = new Point(Form1.WinPos_X, Form1.WinPos_Y);
            this.Text = "Display Mode";

            if (Form1.DSP_flag == 1)
            {
                Temp_text = "Selected display mode: TopMost";
            }
            else
            {
                Temp_text = "Selected display mode: Normal";
            }

            label = new Label()
            {
                Location = new Point(20, 20),
                Text = "Select the display mode",
                Size = new Size(200, 24),
            };

            button_A = new Button()
            {
                Text = "Normal",
                Location = new Point(50, 80),
                AutoSize = true,
            };
            button_A.Click += new EventHandler(A_button_CLicked);

            button_B = new Button()
            {
                Text = "Topmost",
                Location = new Point(150, 80),
                AutoSize = true,
            };
            button_B.Click += new EventHandler(B_button_CLicked);

            label_Sel = new Label()
            {
                Text = Temp_text,
                Location = new Point(50, 120),
                Size = new Size(200, 25),
            };

            close_button = new Button()
            {
                Text = "Close",
                Location = new Point(150, 200),
            };
            close_button.Click += new EventHandler(Close_button_CLicked);

            this.Controls.AddRange(new Control[]
            {
                label, label_Sel, button_A, button_B, close_button
            });
        }

        void A_button_CLicked(object sender, EventArgs e)
        {
            Form1.DSP_flag = 0;
            Form1.DSP_change_flag = 1;
            label_Sel.Text = "Selected display mode: Normal";
        }

        void B_button_CLicked(object sender, EventArgs e)
        {
            Form1.DSP_flag = 1;
            Form1.DSP_change_flag = 1;
            label_Sel.Text = "Selected display mode: TopMost";
        }

        void Close_button_CLicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class Dialog_PhysicsMode : Form      // Select the Display Mode
    {
        Label label;
        Label label_Sel;
        Button button_A;
        Button button_B;
        Button button_C;
        Button close_button;
        public string Temp_text;

        public Dialog_PhysicsMode()
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(300, 300);
            this.Location = new Point(Form1.WinPos_X, Form1.WinPos_Y);
            this.Text = "Physics calc. mode";

            Temp_text = "Selected: No Calc.";
            if (Form1.Phy_flag == -1)
            {
                Temp_text = "Selected: Realtime Calc.";
            }
            if (Form1.Phy_flag == 1)
            {
                Temp_text = "Selected: Calc. at loading";
            }

            label = new Label()
            {
                Location = new Point(20, 20),
                Text = "Select the physics calc. mode",
                Size = new Size(200, 24),
            };

            button_A = new Button()
            {
                Text = "No calc.",
                Location = new Point(10, 80),
                Size = new Size(80, 25),
            };
            button_A.Click += new EventHandler(A_button_CLicked);

            button_B = new Button()
            {
                Text = "Realtime",
                Location = new Point(100, 80),
                Size = new Size(80, 25),
            };
            button_B.Click += new EventHandler(B_button_CLicked);

            button_C = new Button()
            {
                Text = "At loading",
                Location = new Point(190, 80),
                Size = new Size(80, 25),
            };
            button_C.Click += new EventHandler(C_button_CLicked);

            label_Sel = new Label()
            {
                Text = Temp_text,
                Location = new Point(20, 120),
                Size = new Size(200, 25),
            };

            close_button = new Button()
            {
                Text = "Close",
                Location = new Point(150, 200),
            };
            close_button.Click += new EventHandler(Close_button_CLicked);

            this.Controls.AddRange(new Control[]
            {
                label, label_Sel, button_A, button_B, button_C, close_button
            });
        }

        void A_button_CLicked(object sender, EventArgs e)
        {
            Form1.Phy_flag = 0;
            Form1.Phy_change_flag = 1;
            label_Sel.Text = "Selected: No Calc.";
        }

        void B_button_CLicked(object sender, EventArgs e)
        {
            Form1.Phy_flag = -1;
            Form1.Phy_change_flag = 1;
            label_Sel.Text = "Selected: Realtime";
        }

        void C_button_CLicked(object sender, EventArgs e)
        {
            Form1.Phy_flag = 1;
            Form1.Phy_change_flag = 1;
            label_Sel.Text = "Selected: At loading";
        }

        void Close_button_CLicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class Dialog_Schedule : Form           // Speed Control
    {
        Label label_Sch;
        Button Schedule_button;
        Button Close_button;
        string Temp_Text;

        public Dialog_Schedule()
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = true;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(300, 300);
            this.Location = new Point(Form1.WinPos_X, Form1.WinPos_Y);
            this.Text = "ON/OFF the Animation Schedule";

            Schedule_button = new Button()
            {
                Text = "Schedule ON/OFF",
                Location = new Point(50, 50),
                Size = new Size(150, 30),
            };
            Schedule_button.Click += new EventHandler(Schedule_button_CLicked);

            if (Form1.Schedule_flag == 1)
            {
                Temp_Text = "Schedule: ON ";
            }
            else
            {
                Temp_Text = "Schedule: OFF";
            }
            label_Sch = new Label()
            {
                Location = new Point(50, 120),
                Text = Temp_Text,
            };
            label_Sch.Size = new Size(200, 25);

            Close_button = new Button()
            {
                Text = "Close",
                Location = new Point(150, 200),
            };
            Close_button.Click += new EventHandler(Close_button_CLicked);

            this.Controls.AddRange(new Control[] {
                Schedule_button, label_Sch, Close_button
            });
        }

        void Schedule_button_CLicked(object sender, EventArgs e)
        {
            if (Form1.Schedule_flag == 1)
            {
                label_Sch.Text = "Schedule: OFF";
                Form1.MotionMode = 1;
                Form1.Schedule_flag = 0;

            }
            else
            {
                label_Sch.Text = "Schedule: ON ";
                Form1.MotionMode = -1;
                Form1.Schedule_flag = 1;
            }
        }

        void Close_button_CLicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class Dialog_WindowPos : Form      // Set the Position for the Information Window
    {
        Label label_X;
        Label label_Y;
        public TextBox textBox_X;
        public TextBox textBox_Y;
        Button okButton;

        public Dialog_WindowPos()
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(300, 300);
            this.Location = new Point(Form1.WinPos_X, Form1.WinPos_Y);
            this.Text = "Set the position for the information window";

            label_X = new Label()
            {
                Location = new Point(20, 30),
                Text = "X_Position ＝ ",
            };
            textBox_X = new TextBox()
            {
                Text = Form1.WinPos_X.ToString(),
                Location = new Point(120, 30),
            };

            label_Y = new Label()
            {
                Location = new Point(20, 80),
                Text = "Y_Position ＝ ",
            };
            textBox_Y = new TextBox()
            {
                Text = Form1.WinPos_Y.ToString(),
                Location = new Point(120, 80),
            };

            okButton = new Button()
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(150, 200),
            };

            this.Controls.AddRange(new Control[]
            {
                label_X, label_Y, textBox_X, textBox_Y, okButton
            });
        }
    }

    public class CSV_Read      // CSV Read
    {
        string[] Temp_Data = new string[10];
        int Model_Count = 0;
        int Motion_Count = 0;
        int Schedule_Count = 0;
        public void Read_CSV()
        {
            StreamReader sr = new StreamReader(@"Data/DeskTopMascot_Setting.csv", Encoding.GetEncoding("Shift_JIS"));
            try
            {
                while (sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        Temp_Data[i] = values[i];
                    }

                    if (Temp_Data[0] == "Model")
                    {
                        if (Model_Count >= Form1.models_url.Length)
                        {
                            Array.Resize(ref Form1.models_url, Form1.models_url.Length + 1);
                            Array.Resize(ref Form1.models_name, Form1.models_name.Length + 1);
                        }
                        Form1.models_url[Model_Count] = Temp_Data[1];
                        Form1.models_name[Model_Count] = Temp_Data[2];
                        Model_Count = Model_Count + 1;
                        Form1.MaxModels = Model_Count;
                    }
                    if (Temp_Data[0] == "Motion")
                    {
                        if (Motion_Count >= Form1.motions_name.Length)
                        {
                            Array.Resize(ref Form1.motions_no, Form1.motions_no.Length + 1);
                            Array.Resize(ref Form1.motions_name, Form1.motions_name.Length + 1);
                        }
                        if (Temp_Data[1] == "")
                        {
                            Temp_Data[1] = "0";
                        }
                        Form1.motions_no[int.Parse(Temp_Data[1])] = Temp_Data[1];
                        Form1.motions_name[int.Parse(Temp_Data[1])] = Temp_Data[2];
                        Motion_Count = Motion_Count + 1;
                        Form1.MaxMotions = Motion_Count;
                    }
                    if (Temp_Data[0] == "Schedule")
                    {
                        if (Schedule_Count >= Form1.schedule_SH.Length)
                        {
                            Array.Resize(ref Form1.schedule_SH, Form1.schedule_SH.Length + 1);
                            Array.Resize(ref Form1.schedule_SM, Form1.schedule_SM.Length + 1);
                            Array.Resize(ref Form1.schedule_MN, Form1.schedule_MN.Length + 1);
                            Array.Resize(ref Form1.schedule_SP, Form1.schedule_SP.Length + 1);
                            Array.Resize(ref Form1.schedule_PH, Form1.schedule_PH.Length + 1);
                            Array.Resize(ref Form1.schedule_CM, Form1.schedule_CM.Length + 1);
                            Array.Resize(ref Form1.schedule_AD1, Form1.schedule_AD1.Length + 1);
                            Array.Resize(ref Form1.schedule_AD2, Form1.schedule_AD2.Length + 1);
                        }
                        if (Temp_Data[1] == "")
                        {
                            Temp_Data[1] = "0";
                        }
                        if (Temp_Data[2] == "")
                        {
                            Temp_Data[2] = "0";
                        }
                        if (Temp_Data[3] == "")
                        {
                            Temp_Data[3] = "0";
                        }
                        if (Temp_Data[4] == "")
                        {
                            Temp_Data[4] = "0.6";
                        }
                        if (Temp_Data[5] == "")
                        {
                            Temp_Data[5] = "ON";
                        }
                        if (Temp_Data[6] == "")
                        {
                            Temp_Data[6] = "One-Shot";
                        }
                        if (Temp_Data[7] == "")
                        {
                            Temp_Data[7] = "0";
                        }
                        if (Temp_Data[8] == "")
                        {
                            Temp_Data[8] = "0";
                        }
                        Form1.schedule_SH[Schedule_Count] = Temp_Data[1];
                        Form1.schedule_SM[Schedule_Count] = Temp_Data[2];
                        Form1.schedule_MN[Schedule_Count] = Temp_Data[3];
                        Form1.schedule_SP[Schedule_Count] = Temp_Data[4];
                        Form1.schedule_PH[Schedule_Count] = Temp_Data[5];
                        Form1.schedule_CM[Schedule_Count] = Temp_Data[6];
                        Form1.schedule_AD1[Schedule_Count] = Temp_Data[7];
                        Form1.schedule_AD2[Schedule_Count] = Temp_Data[8];
                        Schedule_Count = Schedule_Count + 1;
                        Form1.MaxSchedule = Schedule_Count;
                    }
                }
            }
            finally
            {
                sr.Close();
            }
        }
    }

    public class CSV_Save      // CSV Save
    {
        string[] Temp_Data = new string[10];

        public void Save_CSV()
        {
            try
            {
                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                StreamWriter file = new StreamWriter(@"Data/DeskTopMascot_Setting.csv", false, sjisEnc);
                file.WriteLine("Item" + "," + "MMD_Model_Data" + "," + "Model_Name" + "," + " " + "," + " ");
                for (int i = 0; i < Form1.models_url.Length; i++)
                {
                    file.WriteLine("Model" + "," + Form1.models_url[i] + "," + Form1.models_name[i] + "," + " " + "," + " ");
                }
                file.WriteLine(" " + "," + " " + "," + " " + "," + " " + "," + " ");

                file.WriteLine("Item" + "," + "Motion_No" + "," + "Note" + "," + "Count" + "," + " ");
                for (int i = 0; i < Form1.motions_name.Length; i++)
                {
                    file.WriteLine("Motion" + "," + Form1.motions_no[i] + "," + Form1.motions_name[i] + "," + " " + "," + " ");
                }
                file.WriteLine(" " + "," + " " + "," + " " + "," + " " + "," + " ");

                file.WriteLine("Item" + "," + "Start Time" + "," + "Start Time" + "," + "Motion" + "," + "Motion" + "," + "Physics" + "," + "Motion" + "," + "Motion" + "," + "Motion");
                file.WriteLine("Item" + "," + "Hour" + "," + "Minutes" + "," + "No." + "," + "Speed" + "," + "calc ON/OFF" + "," + "Command" + "," + "Additional" + "," + "Additional"); ; ;
                file.WriteLine("Note" + "," + "," + "," + "," + "0.6" + "," + ": Normal Speed" + "," + "," + ",");
                file.WriteLine("Note" + "," + "," + "," + "," + "," + "," + "One-Shot" + "," + ",");
                file.WriteLine("Note" + "," + "," + "," + "," + "," + "," + "Loop" + "," + ",");
                file.WriteLine("Note" + "," + "," + "," + "," + "," + "," + "," + "Random" + ",");
                for (int i = 0; i < Form1.schedule_SH.Length; i++)
                {
                    file.WriteLine("Schedule" + "," + Form1.schedule_SH[i] + "," + Form1.schedule_SM[i] + "," + Form1.schedule_MN[i] + "," + Form1.schedule_SP[i] + "," + Form1.schedule_PH[i] + "," + Form1.schedule_CM[i] + "," + Form1.schedule_AD1[i] + "," + Form1.schedule_AD2[i]);
                }

                file.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Save Error: " + e.Message);
            }
        }
    }

}
