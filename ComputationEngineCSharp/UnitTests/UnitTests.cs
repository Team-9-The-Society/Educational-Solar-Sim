using System;
using NUnit.Framework;
using ComputationEngineCSharp;

namespace UnitTests
{
    public class Tests
    {
        readonly NBody nBody = new NBody();
        double gravitationalConstant;
        double roundedGravitationalConstant;

        [OneTimeSetUp]
        public void SetUp()
        {
            gravitationalConstant = nBody.GravitationalConstant;
            roundedGravitationalConstant = nBody.RoundToSignificantDigits(gravitationalConstant, 15);
        }

        [Test]
        public void NegAndZeroMassTest()
        {
            // Mass 1 at (0, 0, 0) is -1 kg
            // Mass 2 at (1, 0, 0) is 1 kg
            double[,] PosPass = new double[2, 3] { { 0, 0, 0 }, { 1, 0, 0 } };
            double[] massPass = new double[2] { -1, 1 };

            var ex = Assert.Throws<ArgumentException>(() => nBody.UpdateForce(PosPass, massPass, 2));
            Assert.That(ex.Message, Is.EqualTo("Mass can not be 0 or negative"));

            // Mass 1 at (0, 0, 0) is 1 kg
            // Mass 2 at (1, 0, 0) is -1 kg
            massPass = new double[2] { 1, -1 };

            ex = Assert.Throws<ArgumentException>(() => nBody.UpdateForce(PosPass, massPass, 2));
            Assert.That(ex.Message, Is.EqualTo("Mass can not be 0 or negative"));

            // Mass 1 at (0, 0, 0) is 0 kg
            // Mass 2 at (1, 0, 0) is 1 kg
            massPass = new double[2] { 0, 1 };

            ex = Assert.Throws<ArgumentException>(() => nBody.UpdateForce(PosPass, massPass, 2));
            Assert.That(ex.Message, Is.EqualTo("Mass can not be 0 or negative"));

            // Mass 1 at (0, 0, 0) is 1 kg
            // Mass 2 at (1, 0, 0) is 0 kg
            massPass = new double[2] { 1, 0 };

            ex = Assert.Throws<ArgumentException>(() => nBody.UpdateForce(PosPass, massPass, 2));
            Assert.That(ex.Message, Is.EqualTo("Mass can not be 0 or negative"));
        }

        [Test]
        public void SimpleXTest()
        {
            // Mass 1 at (0, 0, 0) is 1 kg
            // Mass 2 at (1, 0, 0) is 1 kg
            double[,] PosPass = new double[2, 3] { { 0, 0, 0 }, { 1, 0, 0 } };
            double[] massPass = new double[2] { 1, 1 };

            double[,] expected = new double[2, 3] { {roundedGravitationalConstant, 0, 0 }, { -1 * roundedGravitationalConstant, 0, 0 } };
            double[,] result = nBody.UpdateForce(PosPass, massPass, 2);
            Assert.AreEqual(expected, result);

            // Mass 2 at (-1, 0, 0) is 1 kg
            PosPass = new double[2, 3] { { 0, 0, 0 }, { -1, 0, 0 } };

            expected = new double[2, 3] { { -1 * roundedGravitationalConstant, 0, 0 }, { roundedGravitationalConstant, 0, 0 } };
            result = nBody.UpdateForce(PosPass, massPass, 2);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SimpleYTest()
        {
            // Mass 1 at (0, 0, 0) is 1 kg
            // Mass 2 at (0, 1, 0) is 1 kg
            double[,] PosPass = new double[2, 3] { { 0, 0, 0 }, { 0, 1, 0 } };
            double[] massPass = new double[2] { 1, 1 };

            double[,] expected = new double[2, 3] { { 0, roundedGravitationalConstant, 0 }, { 0, -1 * roundedGravitationalConstant, 0 } };
            double[,] result = nBody.UpdateForce(PosPass, massPass, 2);
            Assert.AreEqual(expected, result);

            // Mass 2 at (0, -1, 0) is 1 kg
            PosPass = new double[2, 3] { { 0, 0, 0 }, { 0, -1, 0 } };

            expected = new double[2, 3] { { 0, -1 * roundedGravitationalConstant, 0 }, { 0, roundedGravitationalConstant, 0 } };
            result = nBody.UpdateForce(PosPass, massPass, 2);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SimpleZTest()
        {
            // Mass 1 at (0, 0, 0) is 1 kg
            // Mass 2 at (0, 1, 0) is 1 kg
            double[,] PosPass = new double[2, 3] { { 0, 0, 0 }, { 0, 0, 1 } };
            double[] massPass = new double[2] { 1, 1 };

            double[,] expected = new double[2, 3] { { 0, 0, roundedGravitationalConstant }, { 0, 0, -1 * roundedGravitationalConstant } };
            double[,] result = nBody.UpdateForce(PosPass, massPass, 2);
            Assert.AreEqual(expected, result);

            // Mass 2 at (0, 0, -1) is 1 kg
            PosPass = new double[2, 3] { { 0, 0, 0 }, { 0, 0, -1 } };

            expected = new double[2, 3] { { 0, 0, -1 * roundedGravitationalConstant }, { 0, 0, roundedGravitationalConstant } };
            result = nBody.UpdateForce(PosPass, massPass, 2);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SimpleXYZTest()
        {
            // Mass 1 at (0, 0, 0) is 1 kg
            // Mass 2 at (1, 1, 1) is 1 kg
            double[,] PosPass = new double[2, 3] { { 0, 0, 0 }, { 1, 1, 1 } };
            double[] massPass = new double[2] { 1, 1 };

            double xyzComponentForce = nBody.RoundToSignificantDigits(gravitationalConstant / (3 * Math.Sqrt(3)), 15);

            double[,] expected = new double[2, 3] { { xyzComponentForce, xyzComponentForce, xyzComponentForce }, { -1 * xyzComponentForce, -1 * xyzComponentForce, -1 * xyzComponentForce } };
            double[,] result = nBody.UpdateForce(PosPass, massPass, 2);
            Assert.AreEqual(expected, result);

            // Mass 2 at (-1, -1, -1) is 1 kg
            PosPass = new double[2, 3] { { 0, 0, 0 }, { -1, -1, -1 } };

            expected = new double[2, 3] { { -1 * xyzComponentForce, -1 * xyzComponentForce, -1 * xyzComponentForce }, { xyzComponentForce, xyzComponentForce, xyzComponentForce } };
            result = nBody.UpdateForce(PosPass, massPass, 2);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SunAndEarthTest()
        {
            // Mass 1 (Sun) at (0, 0, 0) is 1.98855E30 kg
            // Mass 2 (Earth) at (1 AU, 0, 0) is 5.9722E24 kg
            double[,] PosPass = new double[2, 3] { { 0, 0, 0 }, { 1, 0, 0 } };
            double[] massPass = new double[2] { 1.98855 * Math.Pow(10, 30), 5.9722 * Math.Pow(10, 24) };

            double forceBetweenSunAndEarth = nBody.RoundToSignificantDigits(3.5437489 * Math.Pow(10, 22), 3);

            double[,] expected = new double[2, 3] { { forceBetweenSunAndEarth, 0, 0 }, { -1 * forceBetweenSunAndEarth, 0, 0 } };
            double[,] result = nBody.UpdateForce(PosPass, massPass, 2, "AU");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result[i, j] = nBody.RoundToSignificantDigits(result[i, j], 3);
                }
            }
            Assert.AreEqual(expected, result);
        }
    }
}