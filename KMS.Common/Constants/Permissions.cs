using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS.Common.Constants
{
    public static class Permissions
    {
        [Description("Người dùng")]
        public static class User
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.User.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.User.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.User.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.User.Update";

            [Description("Xóa")] 
            public const string Remove = "Pms.User.Remove";
        }

        [Description("Phân quyền")]
        public static class Role
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.Role.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.Role.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.Role.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.Role.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.Role.Remove";
        }

        [Description("Mẫu Form")]
        public static class FormDemo
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.FormDemo.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.FormDemo.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.FormDemo.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.FormDemo.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.FormDemo.Remove";
        }

        [Description("Chi tiết Mẫu Form")]
        public static class FormDemoDetail
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.FormDemoDetail.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.FormDemoDetail.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.FormDemoDetail.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.FormDemoDetail.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.FormDemoDetail.Remove";
        }

        [Description("Ký túc xá")]
        public static class Dormitory
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.Dormitory.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.Dormitory.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.Dormitory.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.Dormitory.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.Dormitory.Remove";
        }
        [Description("Sách")]
        public static class Book
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.Book.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.Book.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.Book.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.Book.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.Book.Remove";
        }

        [Description("Khách hàng")]
        public static class Customer
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.Customer.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.Customer.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.Customer.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.Customer.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.Customer.Remove";
        }

        [Description("Sinh viên")]
        public static class Student
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.Student.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.Student.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.Student.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.Student.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.Student.Remove";
        }

        public static class Company
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.Company.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.Company.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.Company.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.Company.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.Company.Remove";
        }

        public static class Application
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.Application.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.Application.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.Application.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.Application.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.Application.Remove";
        }

        public static class Schema
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.Schema.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.Schema.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.Schema.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.Schema.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.Schema.Remove";
        }

        public static class Table
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.Table.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.Table.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.Table.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.Table.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.Table.Remove";
        }

        public static class TableDetail
        {
            [Description("Xem danh sách")]
            public const string View = "Pms.TableDetail.View";

            [Description("Xem chi tiết")]
            public const string ViewDetail = "Pms.TableDetail.ViewDetail";

            [Description("Thêm mới")]
            public const string Insert = "Pms.TableDetail.Insert";

            [Description("Cập nhật")]
            public const string Update = "Pms.TableDetail.Update";

            [Description("Xóa")]
            public const string Remove = "Pms.TableDetail.Remove";
        }
    }
}