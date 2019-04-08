using System;
using System.Linq.Expressions;

namespace MathLib
{
    public static class MathExtentions
    {
        public static T Add<T>(this T start, params T[] these)
        {
            return Express<T>(Expression.Add, start, these);
        }
        public static T Subtract<T>(this T start, params T[] these)
        {
            return Express<T>(Expression.Subtract, start, these);
        }
        public static T Multiply<T>(this T start, params T[] these)
        {
            return Express<T>(Expression.Multiply, start, these);
        }
        public static T Divide<T>(this T start, params T[] these)
        {
            return Express<T>(Expression.Divide, start, these);
        }
        public static T Modulo<T>(this T start, params T[] these)
        {
            return Express<T>(Expression.Modulo, start, these);
        }
        public static T Power<T>(this T start, params T[] these)
        {
            return Express<T>(Expression.Power, start, these);
        }

        private static T Express<T>(Func<Expression, Expression, BinaryExpression> epress, T start, params T[] these)
        {
            var val1 = Expression.Parameter(typeof(T));
            var val2 = Expression.Parameter(typeof(T));

            var lamb = Expression.Lambda<Func<T, T, T>>(
                epress(val1, val2),
                new ParameterExpression[] { val1, val2 }
            ).Compile();

            T s = start;
            try
            {
                for (int i = 0; i < these.Length; i++)
                {
                    s = lamb(s, these[i]);
                }
                
                return (T)s;
            }
            catch
            {
                return default(T);
            }
        }
    }
}
