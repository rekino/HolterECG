using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HolterECG.DataStructures;

namespace UnitTests
{
    [TestClass]
    public class PatientUnitTest
    {
        [TestMethod]
        public void Constructor()
        {
            Patient patient = new Patient();
            Assert.IsNotNull(patient);
        }

        [TestMethod]
        public void Serialize()
        {
            Patient patient = new Patient()
            {
                Age = 28,
                FirstName = "romain",
                Height = 178,
                Insurance = "Parsian",
                LastName = "hoogmoed",
                Medications = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.",
                MoreInformation = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.",
                PhoneNumber = "(656)-976-4980",
                Physician = "Veronica Crawford",
                Sex = Sex.Male,
                Symptoms = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.",
                Weight = 70
            };

            string testXml = patient.Serialize();
            string trueXml = "<Patient><FirstName>romain</FirstName><LastName>hoogmoed</LastName><PhoneNumber>(656)-976-4980</PhoneNumber><Height>178</Height><Weight>70</Weight><Physician>Veronica Crawford</Physician><Sex>Male</Sex><Age>28</Age><Insurance>Parsian</Insurance><Symptoms>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.</Symptoms><Medications>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.</Medications><MoreInformation>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.</MoreInformation></Patient>";

            Assert.AreEqual(trueXml, testXml);
        }
    }
}
