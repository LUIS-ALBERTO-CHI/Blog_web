using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FwaEu.Fwamework.Reflection
{
	public static class ExpressionHelper
	{
		private class ResolveParameterVisitor : ExpressionVisitor
		{
			protected readonly ParameterExpression _param;
			protected readonly object _value;

			public ResolveParameterVisitor(ParameterExpression param, object value)
			{
				_param = param;
				_value = value;
			}

			protected override Expression VisitParameter(ParameterExpression node)
			{
				if (node.Type == _param.Type && node.Name == _param.Name && TypeHelper.IsSimpleType(node.Type))
					return Expression.Constant(_value);

				return base.VisitParameter(node);
			}

			protected override Expression VisitLambda<T>(Expression<T> node)
			{
				var parameters = node.Parameters.Where(p => p.Name != _param.Name && p.Type != _param.Type).ToList();
				return Expression.Lambda(Visit(node.Body), parameters);
			}
		}

		private class ResolveParameterVisitor<TOut> : ResolveParameterVisitor
		{
			public ResolveParameterVisitor(ParameterExpression param, object value)
				:base(param, value)
			{
			}

			protected override Expression VisitLambda<T>(Expression<T> node)
			{
				var parameters = node.Parameters.Where(p => p.Name != _param.Name && p.Type != _param.Type).ToList();
				return Expression.Lambda<TOut>(Visit(node.Body), parameters);
			}
		}

		public static LambdaExpression ResolveExpressionParameter(Expression lambdaExpression, ParameterExpression parameter, object parameterValue)
		{
			return new ResolveParameterVisitor(parameter, parameterValue).Visit(lambdaExpression) as LambdaExpression;
		}

		public static Expression<TOut> ResolveExpressionParameter<TOut>(Expression lambdaExpression, ParameterExpression parameter, object parameterValue)
		{
			return new ResolveParameterVisitor<TOut>(parameter, parameterValue).Visit(lambdaExpression) as Expression<TOut>;
		}

		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
		{
			var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
		}

		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
		{
			var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
		}

		private static IOrderedQueryable<TClass> OrderByMethod<TClass>(IQueryable<TClass> query, string methodName, LambdaExpression orderByExpression)
		{
			Type elementType = query.ElementType;

			MethodInfo orderByMethod = typeof(Queryable).GetMethods()
				.Where(method => method.Name == methodName && method.GetParameters().Length == 2)
				.Single()
				.MakeGenericMethod(elementType, orderByExpression.ReturnType);

			return (IOrderedQueryable<TClass>)orderByMethod.Invoke(null, new object[] { query, orderByExpression });
		}

		public static IOrderedQueryable<TClass> OrderBy<TClass>(this IQueryable<TClass> query, LambdaExpression orderByExpression)
		{
			return OrderByMethod(query, "OrderBy", orderByExpression);
		}

		public static IOrderedQueryable<TClass> OrderByDescending<TClass>(this IQueryable<TClass> query, LambdaExpression orderByExpression)
		{
			return OrderByMethod(query, "OrderByDescending", orderByExpression);
		}

		private static IOrderedQueryable<TClass> ThenByMethod<TClass>(IOrderedQueryable<TClass> query, string methodName, LambdaExpression orderByExpression)
		{
			Type elementType = query.ElementType;

			MethodInfo orderByMethod = typeof(Queryable).GetMethods()
				.Where(method => method.Name == methodName && method.GetParameters().Length == 2)
				.Single()
				.MakeGenericMethod(elementType, orderByExpression.ReturnType);

			return (IOrderedQueryable<TClass>)orderByMethod.Invoke(null, new object[] { query, orderByExpression });
		}

		public static IOrderedQueryable<TClass> ThenBy<TClass>(this IOrderedQueryable<TClass> query, LambdaExpression orderByExpression)
		{
			return ThenByMethod(query, "ThenBy", orderByExpression);
		}

		public static IOrderedQueryable<TClass> ThenByDescending<TClass>(this IOrderedQueryable<TClass> query, LambdaExpression orderByExpression)
		{
			return ThenByMethod(query, "ThenByDescending", orderByExpression);
		}
	}
}
