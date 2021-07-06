using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Simplex;

namespace UnitTest
{
    [TestClass]
    public class UnitT
    {
        [TestMethod]
        public void Test1()
        {
            VvodZnacheniy vz = new VvodZnacheniy();
            vz.simplex();
            Assert.AreEqual(vz.table_rezultat[vz.table_rezultat.GetLength(0) - 1, 0] , -9,5);
        }
        [TestMethod]
        public void Test2()
        {
            VvodZnacheniy vz = new VvodZnacheniy();
            vz.simplex();
            Assert.AreEqual(vz.table_rezultat[vz.table_rezultat.GetLength(0) - 1, 0] *-1, 9,5);
        }
        [TestMethod]
        public void Test3()
        {
            VvodZnacheniy vz = new VvodZnacheniy();
            vz.simplex();
            Assert.AreEqual(vz.table_rezultat[0,0], 0,8);
        }
        [TestMethod]
        public void Test4()
        {
            VvodZnacheniy vz = new VvodZnacheniy();
            vz.simplex();
            Assert.AreEqual(vz.table_rezultat[2, 0], -1,5);
        }
    }
}
