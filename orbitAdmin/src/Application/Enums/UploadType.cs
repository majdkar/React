using System.ComponentModel;

namespace SchoolV01.Application.Enums
{
    public enum UploadType : byte
    {

        /*s0018s*/
        [Description(@"Images\Sections")] Section,
        [Description(@"Images\AdvanceTypes")] AdvanceType,
        [Description(@"Images\Students")] Student,
        [Description(@"Images\Leaves")] Leave,
        [Description(@"Images\Advances")] Advance,
        [Description(@"Images\Employees")] Employee,
        [Description(@"Images\Owners")] Owner,

        [Description(@"Images\ProfilePictures")] ProfilePicture,


        [Description(@"Documents")] Document,

        [Description(@"Files\UploadFiles\TasksFiles")] TasksFiles,

        [Description(@"Images\Brands")] Brand,
        [Description(@"Images\Members")] Member,
        [Description(@"Images\Leases")] Leases,
        [Description(@"Images\Supplier")] Supplier,
        [Description(@"Images\Products")] Product,
        [Description(@"Images\ProductCategories")] ProductCategory,

    }
}