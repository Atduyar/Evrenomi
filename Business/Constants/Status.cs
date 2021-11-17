using System;
using System.Linq.Expressions;
using Core.Entities.Concrete;

namespace Business.Constants
{
    public static class Status
    {
        public const int Checked = 1;
        public const int Ban = 2;
        public const int Hidden = 4;
        public static readonly int[] Flag = { 8, 16, 32 };
        public static int[] Sban = { 64, 128, 256, 512 };
        public static int Approval = 1024;

        //public static readonly int[] Settings = { 1024, 2048 };


        public static readonly int User = 0;


        public static readonly int DontView = Checked | Ban | Hidden;//
        public static readonly int DontOpen = Checked | Ban;//
        public static readonly int All = Checked | Ban | Hidden | Flag[0] | Flag[1] | Flag[2] /*| Settings[0] | Settings[1] | Settings[2]*/;
        public static readonly int Neno = 0;

        public static int DefBlog = Approval;


        public enum Per
        {
            Me,
            User,
            UnUser,
            Admin,
            System
        };

        //public static int GetBlogMask(Per p = Per.User)
        //{
        //    switch (p)
        //    {
        //        case Per.Me://????
        //        case Per.User:
        //        case Per.UnUser:
        //            return Checked | Ban | Hidden | Sban[0] | Sban[1] | Sban[2];

        //        case Per.Admin:
        //        case Per.System:
        //            return Status.Neno;
        //    }
        //    return Checked | Ban | Hidden;//sssil lan bunu
        //}
        //public static int GetBlogFilter(Per p = Per.User)
        //{
        //    return Status.Neno;//sssil lan bunu
        //    switch (p)
        //    {
        //        case Per.Me://???
        //        case Per.User:
        //        case Per.UnUser:
        //        case Per.Admin:
        //        case Per.System:
        //            return Status.Neno;
        //    }
        //}

        public static int GetBlogByIdMask(Per p = Per.User)
        {
            switch (p)
            {
                case Per.Me://????
                case Per.User:
                case Per.UnUser:
                    return Checked | Ban | Hidden | Sban[0] | Sban[1] | Sban[2];

                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
            return Checked | Ban | Hidden;//sssil lan bunu
        }
        public static int GetBlogByIdFilter(Per p = Per.User)
        {
            return Status.Neno | Status.Approval;//onaylı olmayanlarıda getir
            switch (p)
            {
                case Per.Me://???
                case Per.User:
                case Per.UnUser:
                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
        }

        public static int GetBlogBySearchMask(Per p = Per.User)
        {
            switch (p)
            {
                case Per.Me://????
                case Per.User:
                case Per.UnUser:
                    return Checked | Ban | Hidden | Sban[0] | Sban[1] | Sban[2];

                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
            return Checked | Ban | Hidden;//sssil lan bunu
        }
        public static int GetBlogBySearchFilter(Per p = Per.User)
        {
            return Status.Neno | Status.Approval;//onaylı olmayanlarıda getir
            switch (p)
            {
                case Per.Me://???
                case Per.User:
                case Per.UnUser:
                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
        }

        public static int GetBlogByAuthorIdMask(Per p = Per.User)
        {
            switch (p)
            {
                case Per.Me://????
                case Per.User:
                case Per.UnUser:
                    return Checked | Ban | Hidden | Sban[0] | Sban[1] | Sban[2];

                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
            return Checked | Ban | Hidden;//sssil lan bunu
        }
        public static int GetBlogByAuthorIdFilter(Per p = Per.User)
        {
            return Status.Neno;//sssil lan bunu
            switch (p)
            {
                case Per.Me://???
                case Per.User:
                case Per.UnUser:
                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
        }

        public static int GetBlogPageMask(Per p = Per.User)
        {
            switch (p)
            {
                case Per.Me://????
                case Per.User:
                case Per.UnUser:
                    return Checked | Ban | Hidden | Sban[0] | Sban[1] | Sban[2] | Approval;

                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
            return Checked | Ban | Hidden;//sssil lan bunu
        }
        public static int GetBlogPageFilter(Per p = Per.User)
        {
            return Status.Neno;//sssil lan bunu
            switch (p)
            {
                case Per.Me://???
                case Per.User:
                case Per.UnUser:
                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
        }

        public static int GetBlogByUserReadedMask(Per p = Per.User)
        {
            switch (p)
            {
                case Per.Me://????
                case Per.User:
                case Per.UnUser:
                    return Checked | Ban | Hidden | Sban[0] | Sban[1] | Sban[2];

                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
            return Checked | Ban | Hidden;//sssil lan bunu
        }
        public static int GetBlogByUserReadedFilter(Per p = Per.User)
        {
            return Status.Neno;//sssil lan bunu
            switch (p)
            {
                case Per.Me://???
                case Per.User:
                case Per.UnUser:
                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
        }
        
        public static int GetUserMask(Per p = Per.User)
        {
            switch (p)
            {
                case Per.User:
                case Per.UnUser:
                    return Hidden;

                case Per.Me:
                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
            return Checked | Ban | Hidden;//sssil lan bunu
        }
        public static int GetUserFilter(Per p = Per.User)
        {
            return Status.Neno;//sssil lan bunu
            switch (p)
            {
                case Per.Me:
                case Per.User:
                case Per.UnUser:
                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
        }

        public static int GetUserUpdateMask(Per p = Per.User)
        {
            switch (p)
            {
                case Per.User:
                case Per.Me:
                    return Sban[1];

                case Per.UnUser:
                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
            return Sban[1];//sssil lan bunu
        }
        public static int GetUseUpdateFilter(Per p = Per.User)
        {
            return Status.Neno;//sssil lan bunu
            switch (p)
            {
                case Per.Me:
                case Per.User:
                case Per.UnUser:
                case Per.Admin:
                case Per.System:
                    return Status.Neno;
            }
        }
    }
}