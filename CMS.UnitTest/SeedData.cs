//using KHCN.Data.Entities;
//using KHCN.Data.Repository;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Assert = NUnit.Framework.Assert;

//namespace CMS.UnitTest
//{
//    [TestClass]
//    public class SeedData
//    {
//        private readonly IModuleRepository _moduleRepository;
//        private readonly IFunctionRepository _functionRepository;
//        private readonly IPageRepository _pageRepository;

//        public SeedData(IModuleRepository moduleRepository, IFunctionRepository functionRepository, IPageRepository pageRepository)
//        {
//            _moduleRepository = moduleRepository;
//            _functionRepository = functionRepository;
//            _pageRepository = pageRepository;
//        }

//        [TestMethod]
//        public void SeedModule()
//        {
//            // Arrange
//            var now = DateTime.Now;
//            var lstModule = new List<CMS_Module>
//            {
//                new CMS_Module { Name = "Quản trị hệ thống", Description = "Quản trị hệ thống", IsActive = true, CreatedDate = now, CreatedBy = "host", UpdatedDate = now, UpdatedBy = "host" },
//                new CMS_Module { Name = "Quản trị nội dung", Description = "Quản trị nội dung", IsActive = true, CreatedDate = now, CreatedBy = "host", UpdatedDate = now, UpdatedBy = "host" },
//            };

//            // Act
//            foreach (var item in lstModule)
//            {
//                _moduleRepository.Add(item);
//            }

//            // Assert
//            var data = _moduleRepository.GetAll();
//            //Assert.AreEqual(data);
//        }
//    }
//}