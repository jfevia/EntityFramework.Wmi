using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;

namespace System.Data.WMI.EF6.Gen
{
    internal sealed class SqlChecker : DbExpressionVisitor<bool>
    {
        private SqlChecker()
        {
        }

        public override bool Visit(DbAndExpression expression)
        {
            return VisitBinaryExpression(expression);
        }

        public override bool Visit(DbApplyExpression expression)
        {
            throw new NotSupportedException("apply expression");
        }

        public override bool Visit(DbArithmeticExpression expression)
        {
            return VisitExpressionList(expression.Arguments);
        }

        public override bool Visit(DbCaseExpression expression)
        {
            var flag1 = VisitExpressionList(expression.When);
            var flag2 = VisitExpressionList(expression.Then);
            var flag3 = VisitExpression(expression.Else);

            return flag1 || flag2 || flag3;
        }

        public override bool Visit(DbCastExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbComparisonExpression expression)
        {
            return VisitBinaryExpression(expression);
        }

        public override bool Visit(DbConstantExpression expression)
        {
            return false;
        }

        public override bool Visit(DbCrossJoinExpression expression)
        {
            return VisitExpressionBindingList(expression.Inputs);
        }

        public override bool Visit(DbDerefExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbDistinctExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbElementExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbEntityRefExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbExceptExpression expression)
        {
            var flag1 = VisitExpression(expression.Left);
            var flag2 = VisitExpression(expression.Right);
            return flag1 || flag2;
        }

        public override bool Visit(DbExpression expression)
        {
            throw new NotSupportedException(expression.GetType().FullName);
        }

        public override bool Visit(DbFilterExpression expression)
        {
            var flag1 = VisitExpressionBinding(expression.Input);
            var flag2 = VisitExpression(expression.Predicate);

            return flag1 || flag2;
        }

        public override bool Visit(DbFunctionExpression expression)
        {
            return VisitExpressionList(expression.Arguments);
        }

        public override bool Visit(DbGroupByExpression expression)
        {
            var flag1 = VisitExpression(expression.Input.Expression);
            var flag2 = VisitExpressionList(expression.Keys);
            var flag3 = VisitAggregateList(expression.Aggregates);

            return flag1 || flag2 || flag3;
        }

        public override bool Visit(DbIntersectExpression expression)
        {
            var flag1 = VisitExpression(expression.Left);
            var flag2 = VisitExpression(expression.Right);
            return flag1 || flag2;
        }

        public override bool Visit(DbIsEmptyExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbIsNullExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbIsOfExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbJoinExpression expression)
        {
            var flag1 = VisitExpressionBinding(expression.Left);
            var flag2 = VisitExpressionBinding(expression.Right);
            var flag3 = VisitExpression(expression.JoinCondition);
            return flag1 || flag2 || flag3;
        }

        public override bool Visit(DbLikeExpression expression)
        {
            var flag1 = VisitExpression(expression.Argument);
            var flag2 = VisitExpression(expression.Pattern);
            var flag3 = VisitExpression(expression.Escape);
            return flag1 || flag2 || flag3;
        }

        public override bool Visit(DbLimitExpression expression)
        {
            return VisitExpression(expression.Argument);
        }

        public override bool Visit(DbNewInstanceExpression expression)
        {
            return VisitExpressionList(expression.Arguments);
        }

        public override bool Visit(DbNotExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbNullExpression expression)
        {
            return false;
        }

        public override bool Visit(DbOfTypeExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbOrExpression expression)
        {
            return VisitBinaryExpression(expression);
        }

        public override bool Visit(DbParameterReferenceExpression expression)
        {
            return false;
        }

        public override bool Visit(DbProjectExpression expression)
        {
            var flag1 = VisitExpressionBinding(expression.Input);
            var flag2 = VisitExpression(expression.Projection);
            return flag1 || flag2;
        }

        public override bool Visit(DbPropertyExpression expression)
        {
            return VisitExpression(expression.Instance);
        }

        public override bool Visit(DbQuantifierExpression expression)
        {
            var flag1 = VisitExpressionBinding(expression.Input);
            var flag2 = VisitExpression(expression.Predicate);
            return flag1 || flag2;
        }

        public override bool Visit(DbRefExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbRefKeyExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbRelationshipNavigationExpression expression)
        {
            return VisitExpression(expression.NavigationSource);
        }

        public override bool Visit(DbScanExpression expression)
        {
            return false;
        }

        public override bool Visit(DbSkipExpression expression)
        {
            VisitExpressionBinding(expression.Input);
            VisitSortClauseList(expression.SortOrder);
            VisitExpression(expression.Count);
            return true;
        }

        public override bool Visit(DbSortExpression expression)
        {
            var flag1 = VisitExpressionBinding(expression.Input);
            var flag2 = VisitSortClauseList(expression.SortOrder);
            return flag1 || flag2;
        }

        public override bool Visit(DbTreatExpression expression)
        {
            return VisitUnaryExpression(expression);
        }

        public override bool Visit(DbUnionAllExpression expression)
        {
            return VisitBinaryExpression(expression);
        }

        public override bool Visit(DbVariableReferenceExpression expression)
        {
            return false;
        }

        private bool VisitAggregate(DbAggregate aggregate)
        {
            return VisitExpressionList(aggregate.Arguments);
        }

        private bool VisitAggregateList(IList<DbAggregate> list)
        {
            return VisitList(VisitAggregate, list);
        }

        private bool VisitBinaryExpression(DbBinaryExpression expr)
        {
            var flag1 = VisitExpression(expr.Left);
            var flag2 = VisitExpression(expr.Right);
            return flag1 || flag2;
        }

        private bool VisitExpression(DbExpression expression)
        {
            if (expression == null) return false;
            return expression.Accept(this);
        }

        private bool VisitExpressionBinding(DbExpressionBinding expressionBinding)
        {
            return VisitExpression(expressionBinding.Expression);
        }

        private bool VisitExpressionBindingList(IList<DbExpressionBinding> list)
        {
            return VisitList(VisitExpressionBinding, list);
        }

        private bool VisitExpressionList(IList<DbExpression> list)
        {
            return VisitList(VisitExpression, list);
        }

        private static bool VisitList<TElementType>(ListElementHandler<TElementType> handler, IList<TElementType> list)
        {
            var flag = false;
            foreach (var local in list)
            {
                var flag2 = handler(local);
                flag = flag || flag2;
            }

            return flag;
        }

        private bool VisitSortClause(DbSortClause sortClause)
        {
            return VisitExpression(sortClause.Expression);
        }

        private bool VisitSortClauseList(IList<DbSortClause> list)
        {
            return VisitList(VisitSortClause, list);
        }

        private bool VisitUnaryExpression(DbUnaryExpression expr)
        {
            return VisitExpression(expr.Argument);
        }

        private delegate bool ListElementHandler<in TElementType>(TElementType element);
    }
}