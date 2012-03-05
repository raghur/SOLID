﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SortTests
{
    using SortApp;
    using System.IO;
    [TestClass]
    public class SortTest
    {
        MockRepository mockRepo;
        Mock<IFileWrapper> fileWrapperMock;
        Mock<TextWriter> mockConsole;

        [TestInitialize]
        public void Setup()
        {
            mockRepo = new MockRepository(MockBehavior.Loose);
            fileWrapperMock = mockRepo.Create<IFileWrapper>();
            mockConsole= mockRepo.Create<System.IO.TextWriter>();
            Program.filewrapper = fileWrapperMock.Object;
            Program.writer = mockConsole.Object;
        }

        [TestCleanup]
        public void CleanUp()
        {
            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void TestInputParams()
        {
            mockConsole.Setup(m => m.WriteLine(It.IsAny<String>()));

            int retval = Program.Main (new String[] { });
            Assert.AreEqual<int>(1, retval);
        }

        [TestMethod]
        public void CheckInvalidFile()
        {
            //Arrange
            fileWrapperMock.Setup(m => m.Exists("c:\\nonsense.txt")).Returns(false);
            mockConsole.Setup(m => m.WriteLine(It.IsAny<String>()));
            

            //Act
            int retval = Program.Main(new String[] { "c:\\nonsense.txt" });

            //Assert
            Assert.AreEqual<int>(2, retval);
            
        }

        [TestMethod]
        public void CheckValidFile()
        {
            //Arrange
            fileWrapperMock.Setup(m => m.Exists("c:\\nonsense.txt")).Returns(true);

            //Act
            int retval = Program.Main(new String[] { "c:\\nonsense.txt" });

            //Assert
            Assert.AreEqual<int>(0, retval);
        }

        [TestMethod]
        public void SortData()
        {
            //Arrange
            fileWrapperMock.Setup(m => m.Exists("c:\\nonsense.txt")).Returns(true);
            fileWrapperMock.Setup(m => m.ReadAllLines("c:\\nonsense.txt")).Returns(new string[] {"c", "b", "a"});
            mockConsole.Setup(m => m.WriteLine("a"));
            mockConsole.Setup(m => m.WriteLine("b"));
            mockConsole.Setup(m => m.WriteLine("c"));

            //Act
            int retval = Program.Main(new String[] { "c:\\nonsense.txt" });

            //Assert
            Assert.AreEqual<int>(0, retval);
        }

        [TestMethod]
        public void SortDataEmptyFile()
        {
            //Arrange
            fileWrapperMock.Setup(m => m.Exists("c:\\nonsense.txt")).Returns(true);
            fileWrapperMock.Setup(m => m.ReadAllLines("c:\\nonsense.txt")).Returns(new string[] {});
            // - console should not be invoked - mockConsole.Setup(m => m.WriteLine("a"));
            
            //Act
            int retval = Program.Main(new String[] { "c:\\nonsense.txt" });

            //Assert
            Assert.AreEqual<int>(0, retval);
        }

        [TestMethod]
        public void SortDataCheckCase()
        {
            //Arrange
            fileWrapperMock.Setup(m => m.Exists("c:\\nonsense.txt")).Returns(true);
            fileWrapperMock.Setup(m => m.ReadAllLines("c:\\nonsense.txt")).Returns(new string[] { "A", "a"});
            mockConsole.Setup(m => m.WriteLine("a"));
            mockConsole.Setup(m => m.WriteLine("A"));


            //Act
            int retval = Program.Main(new String[] { "c:\\nonsense.txt" });

            //Assert
            Assert.AreEqual<int>(0, retval);
        }

        [TestMethod]
        public void SortDataCaseInsensitive()
        {
            //Arrange
            fileWrapperMock.Setup(m => m.Exists("c:\\nonsense.txt")).Returns(true);
            fileWrapperMock.Setup(m => m.ReadAllLines("c:\\nonsense.txt")).Returns(new string[] { "A", "a", "b", "B" });
            mockConsole.Setup(m => m.WriteLine("a"));
            mockConsole.Setup(m => m.WriteLine("A"));
            mockConsole.Setup(m => m.WriteLine("b"));
            mockConsole.Setup(m => m.WriteLine("B"));


            //Act
            int retval = Program.Main(new String[] { "c:\\nonsense.txt" });

            //Assert
            Assert.AreEqual<int>(0, retval);
        }
    }
}