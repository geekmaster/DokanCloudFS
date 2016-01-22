﻿/*
The MIT License(MIT)

Copyright(c) 2015 IgorSoft

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IgorSoft.DokanCloudFS.Tests
{
    [TestClass]
    public sealed partial class CloudDirectoryNodeTests
    {
        private Fixture fixture;

        [TestInitialize]
        public void Initialize()
        {
            fixture = Fixture.Initialize();
        }

        [TestMethod, TestCategory(nameof(TestCategories.Offline))]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CloudDirectoryNode_Create_WhereContractIsMissing_Throws()
        {
            var sut = fixture.GetDirectory(null);
        }

        [TestMethod, TestCategory(nameof(TestCategories.Offline))]
        public void CloudDirectoryNode_Create_WhereContractIsSpecified_StoresContract()
        {
            var contract = fixture.TargetDirectory;

            var sut = fixture.GetDirectory(contract);

            Assert.AreEqual(contract, sut.Contract);
        }

        [TestMethod, TestCategory(nameof(TestCategories.Offline))]
        public void CloudDirectoryNode_GetChildItemByName_CallsDriveCorrectly()
        {
            var fileName = fixture.SubDirectoryItems.First().Name;
            var directory = fixture.TargetDirectory;

            fixture.SetupGetChildItems(directory, fixture.SubDirectoryItems);

            var sut = fixture.GetDirectory(directory);
            var result = sut.GetChildItemByName(fixture.Drive, fileName);

            Assert.AreEqual(fileName, result.Name, "Diverging result");

            fixture.VerifyAll();
        }

        [TestMethod, TestCategory(nameof(TestCategories.Offline))]
        public void CloudDirectoryNode_GetChildItems_CallsDriveCorrectly()
        {
            var directory = fixture.TargetDirectory;

            fixture.SetupGetChildItems(directory, fixture.SubDirectoryItems);

            var sut = fixture.GetDirectory(directory);
            var result = sut.GetChildItems(fixture.Drive);

            CollectionAssert.AreEqual(fixture.SubDirectoryItems, result.Select(i => i.Contract).ToArray(), "Diverging result");

            fixture.VerifyAll();
        }

        [TestMethod, TestCategory(nameof(TestCategories.Offline))]
        public void CloudDirectoryNode_Move_Succeeds()
        {
            var contract = fixture.TestDirectory;
            var directory = fixture.TargetDirectory;

            fixture.SetupGetChildItems(directory, fixture.SubDirectoryItems);
            fixture.SetupMove(contract, contract.Name, directory);

            var sut = fixture.GetDirectory(contract);
            sut.Move(fixture.Drive, contract.Name, new CloudDirectoryNode(directory));

            fixture.VerifyAll();
        }

        [TestMethod, TestCategory(nameof(TestCategories.Offline))]
        public void CloudDirectoryNode_MoveAndRename_Succeeds()
        {
            var newName = "RenamedDirectory";
            var contract = fixture.TestDirectory;
            var directory = fixture.TargetDirectory;

            fixture.SetupGetChildItems(directory, fixture.SubDirectoryItems);
            fixture.SetupMove(contract, newName, directory);

            var sut = fixture.GetDirectory(contract);
            sut.Move(fixture.Drive, newName, new CloudDirectoryNode(directory));

            fixture.VerifyAll();
        }

        [TestMethod, TestCategory(nameof(TestCategories.Offline))]
        public void CloudDirectoryNode_NewDirectoryItem_Succeeds()
        {
            var newName = "NewDirectory";
            var contract = fixture.TestDirectory;

            fixture.SetupGetChildItems(contract, fixture.SubDirectoryItems);
            fixture.SetupNewDirectoryItem(contract, newName);

            var sut = fixture.GetDirectory(contract);
            sut.GetChildItems(fixture.Drive);
            var result = sut.NewDirectoryItem(fixture.Drive, newName);

            fixture.VerifyAll();
        }

        [TestMethod, TestCategory(nameof(TestCategories.Offline))]
        public void CloudDirectoryNode_NewFileItem_Succeeds()
        {
            var newName = "NewFile.ext";
            var contract = fixture.TestDirectory;

            fixture.SetupGetChildItems(contract, fixture.SubDirectoryItems);
            fixture.SetupNewFileItem(contract, newName);

            var sut = fixture.GetDirectory(contract);
            sut.GetChildItems(fixture.Drive);
            var result = sut.NewFileItem(fixture.Drive, newName);

            fixture.VerifyAll();
        }

        [TestMethod, TestCategory(nameof(TestCategories.Offline))]
        public void CloudDirectoryNode_Remove_Succeeds()
        {
            var contract = fixture.TestDirectory;

            fixture.SetupRemove(contract);

            var sut = fixture.GetDirectory(contract);
            sut.Remove(fixture.Drive);

            fixture.VerifyAll();
        }
    }
}