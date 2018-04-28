using System.Collections;
using System.Collections.Generic;
using System.Data;
using AutoMapper;


namespace Common
{

    /// <summary>
    ///     AutoMapper扩展帮助类
    /// </summary>
    public static class AutoMapperHelper
    {

        /// <summary>
        ///     类型映射
        /// </summary>
        public static TTarget MapTo<TTarget>(this object source)
        {
            if (source == null) return default(TTarget);
            return Mapper.Map<TTarget>(source);
        }

        /// <summary>
        ///     类型映射
        /// </summary>
        public static TTarget MapTo<TTarget>(this object source,TTarget target)
        {
            if (source == null) return default(TTarget);
            return Mapper.Map(source,target);
        }


        /// <summary>
        ///     集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            foreach (var first in source)
            {
                var type = first.GetType();
                break;
            }
            return Mapper.Map<List<TDestination>>(source);
        }


        /// <summary>
        ///     集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            //IEnumerable<T> 类型需要创建元素的映射
            return Mapper.Map<List<TDestination>>(source);
        }


        /// <summary>
        ///     类型映射
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return destination;
            return Mapper.Map(source, destination);
        }


        /// <summary>
        ///     DataReader映射
        /// </summary>
        public static IEnumerable<T> DataReaderMapTo<T>(this IDataReader reader)
        {
            return Mapper.Map<IDataReader, IEnumerable<T>>(reader);
        }

    }

}