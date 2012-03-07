using System;
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
        Mock<IFileWrapper> mockFileWrapper;
        Mock<TextWriter> mockConsole;
        Program theProgram;
        [TestInitialize]
        public void Setup()
        {
            mockRepo = new MockRepository(MockBehavior.Loose);
            mockFileWrapper = mockRepo.Create<IFileWrapper>();
            mockConsole= mockRepo.Create<System.IO.TextWriter>();
            ISort sorter = new ArraySorter();
            theProgram = new Program(mockFileWrapper.Object, sorter, mockConsole.Object);
            
        }

        [TestCleanup]
        public void CleanUp()
        {
            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void TestInputParams()
        {
            Program.outwriter = mockConsole.Object;
            mockConsole.Setup(m => m.WriteLine(It.IsAny<String>()));

            int retval = Program.Main(new String[] { });
            Assert.AreEqual<int>(1, retval);
        }

        [TestMethod]
        public void CheckInvalidFile()
        {
            //Arrange
            mockFileWrapper.Setup(m => m.Exists()).Returns(false);
            mockConsole.Setup(m => m.WriteLine(It.IsAny<String>()));
            

            //Act
            int retval = theProgram.Main();

            //Assert
            Assert.AreEqual<int>(2, retval);
            
        }

        [TestMethod]
        public void CheckValidFile()
        {
            //Arrange
            mockFileWrapper.Setup(m => m.Exists()).Returns(true);

            //Act
            int retval = theProgram.Main();

            //Assert
            Assert.AreEqual<int>(0, retval);
        }

        [TestMethod]
        public void SortData()
        {
            //Arrange
            mockFileWrapper.Setup(m => m.Exists()).Returns(true);
            mockFileWrapper.Setup(m => m.ReadAllLines()).Returns(new string[] {"c", "b", "a"});
            mockConsole.Setup(m => m.WriteLine("a"));
            mockConsole.Setup(m => m.WriteLine("b"));
            mockConsole.Setup(m => m.WriteLine("c"));

            //Act
            int retval = theProgram.Main();

            //Assert
            Assert.AreEqual<int>(0, retval);
        }

        [TestMethod]
        public void SortDataEmptyFile()
        {
            //Arrange
            mockFileWrapper.Setup(m => m.Exists()).Returns(true);
            mockFileWrapper.Setup(m => m.ReadAllLines()).Returns(new string[] {});
            // - console should not be invoked - mockConsole.Setup(m => m.WriteLine("a"));
            
            //Act
            int retval = theProgram.Main();

            //Assert
            Assert.AreEqual<int>(0, retval);
        }

        [TestMethod]
        public void SortDataCheckCase()
        {
            //Arrange
            mockFileWrapper.Setup(m => m.Exists()).Returns(true);
            mockFileWrapper.Setup(m => m.ReadAllLines()).Returns(new string[] { "A", "a"});
            mockConsole.Setup(m => m.WriteLine("a"));
            mockConsole.Setup(m => m.WriteLine("A"));


            //Act
            int retval = theProgram.Main();

            //Assert
            Assert.AreEqual<int>(0, retval);
        }

        [TestMethod]
        public void SortDataCaseInsensitive()
        {
            //Arrange
            mockFileWrapper.Setup(m => m.Exists()).Returns(true);
            mockFileWrapper.Setup(m => m.ReadAllLines()).Returns(new string[] { "A", "a", "b", "B" });
            mockConsole.Setup(m => m.WriteLine("a"));
            mockConsole.Setup(m => m.WriteLine("A"));
            mockConsole.Setup(m => m.WriteLine("b"));
            mockConsole.Setup(m => m.WriteLine("B"));


            //Act
            int retval = theProgram.Main();

            //Assert
            Assert.AreEqual<int>(0, retval);
        }
        
        [TestMethod]
        public void CheckExceptionHandlingInRead()
        {
            //Arrange
            mockFileWrapper.Setup(m => m.Exists()).Returns(true);
            mockFileWrapper.Setup(m => m.ReadAllLines()).Throws(new IOException("IO Exception occured"));
            mockConsole.Setup(m => m.WriteLine("IO Exception occured"));

            //Act
            int retval = theProgram.Main();

            //Assert
            Assert.AreEqual<int>(3, retval);
            //mock verification of output message print is in the clean up.
        }
    }
}
