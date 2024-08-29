﻿namespace DSL.Evaluator.AST.Expressions.BooleanExpressions.Comparators
{
    internal class Equal : BinaryExpression
    {
        public Equal(IExpression left, IExpression right) : base(left, right)
        {
        }
        protected override object Operate(object left, object right)
        {
            return left.Equals(right);
        }
    }
}
