using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearn.MonoGame.Mechanics
{
    public class Kinematics
    {

        public enum KinematicsType
        {
            Inverse
        }

        private KinematicsType _kinematicsType;

        public Kinematics(KinematicsType kinematicType)
        {
            _kinematicsType = kinematicType;

            Arms = new List<Arm>();

        }

        public Vector2 Position { get; set; }

        public List<Arm> Arms { get; internal set; }

        public void AddArm(int length)
        {

            if (!Arms.Any())
            {
                var arm = new Arm(length);
                arm.Position = Position;
                Arms.Add(arm);
            }
            else
            {
                if (_kinematicsType == KinematicsType.Inverse)
                {
                    var arm = new Arm(length, Arms.Last());
                    arm.Position = arm.Parent.EndPosition;
                    Arms.Add(arm);
                }
            }

        }

        public void Update()
        {
            if (Arms.Any())
            {
                if (_kinematicsType == KinematicsType.Inverse)
                {
                    Arms.Last().Drag(Position);
                }
            }
        }

    }

}
