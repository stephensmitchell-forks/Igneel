﻿using Igneel;
using Igneel.Rendering;
using Igneel.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Igneel;
using Igneel.States;

namespace D3D9Testing
{
    public partial class Form1 : Form
    {
        private float _fps;
        private float _baseTime;

        public Form1(IgneelFormsApp app)
        {
            InitializeComponent();

            app.MainForm = this;
            app.MainControl = screen;
            app.Initialize();

            screen.CreateDafultPresenter();          
            screen.Presenter.Rendering += Presenter_Rendering;
            Engine.Presenter = screen.Presenter;
            screen.IsDefault = true;

            InitializeTests();
        }

        public void InitializeTests()
        {
            testSets.Items.AddRange(TestManager.Tests.ToArray());

            if (TestManager.Tests.Count > 0)
                tests.Items.AddRange(TestManager.Tests[0].Testes.ToArray());
        }

        private void testSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (testSets.SelectedIndex >= 0)
            {
                tests.Items.Clear();
                tests.Items.AddRange(TestManager.Tests[testSets.SelectedIndex].Testes.ToArray());
            }
        }

        private void tests_DoubleClick(object sender, EventArgs e)
        {
            if (tests.SelectedItem != null)
            {
                if (TestManager.Cleaner != null)
                {
                    TestManager.Cleaner();
                    TestManager.Cleaner = null;
                }

                var actionContainer = (ActionContainer)tests.SelectedItem;
                actionContainer.Action();
            }
        }

        private void enableShadowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EngineState.Shadow.Enable = !EngineState.Shadow.Enable;
            enablePhysicsToolStripMenuItem.Checked = !enablePhysicsToolStripMenuItem.Checked;
        }

        private void enablePhysicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Engine.Scene.Physics != null)
            {
                enablePhysicsToolStripMenuItem.Checked = !enablePhysicsToolStripMenuItem.Checked;
                Engine.Scene.Physics.Enable = !Engine.Scene.Physics.Enable;
            }
        }

        void Presenter_Rendering()
        {
            if (_fps == -1)
            {
                _fps = 0;
                _baseTime = Engine.Time.Time;
            }
            else
            {
                float time = Engine.Time.Time;
                if ((time - _baseTime) > 1.0f)
                {
                    fpsCounter.Text = "FPS : " + _fps;
                    _fps = 0;
                    _baseTime = time;
                }
                _fps++;
            }
        }
    }
}
