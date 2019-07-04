using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HumanErrorProject.Engine.Analysis.AbstractSyntaxTree
{
    public class AbstractSyntaxTreeNode
    {
        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                UpdateMembers();
            }
        }

        public IList<AbstractSyntaxTreeNode> Children { get; private set; } = new List<AbstractSyntaxTreeNode>();
        public int Height { get; private set; }
        public int HashTag { get; private set; }
        public bool Terminal => Children.Count == 0;
        public int Degrees => Children.Count;

        public AbstractSyntaxTreeNode(string value)
        {
            Value = value;
        }

        private void UpdateMembers()
        {
            Height = ComputeHeight(Children);
            HashTag = ComputeHashTag(Children, Value, Height);
        }

        private static int ComputeHashTag(IList<AbstractSyntaxTreeNode> nodes, string value, int height)
        {
            var hashtag = -85284014;
            hashtag = hashtag * -1521134295 + value.GetHashCode();
            hashtag = hashtag * -1521134295 + height;
            return nodes.Aggregate(hashtag, (current, node) => current * -1521134295 + node.HashTag);
        }

        private static int ComputeHeight(IList<AbstractSyntaxTreeNode> nodes)
        {
            return nodes.Select(node => node.Height).Concat(new[] { 0 }).Max() + 1;
        }

        public AbstractSyntaxTreeNode Copy()
        {
            return new AbstractSyntaxTreeNode(Value);
        }

        public AbstractSyntaxTreeNode CopyDeep()
        {
            var node = Copy();

            foreach (var astNode in Children)
            {
                node.Children.Add(astNode.CopyDeep());
            }
            node.UpdateMembers();
            return node;
        }

        public void Append(AbstractSyntaxTreeNode node)
        {
            Children.Add(node);
            UpdateMembers();
        }

        public void Prepend(AbstractSyntaxTreeNode node)
        {
            Children.Insert(0, node);
            UpdateMembers();
        }

        public void Remove(AbstractSyntaxTreeNode node)
        {
            if (!Children.Remove(node))
                throw new ArgumentException("Couldn't find item to remove.");
            UpdateMembers();
        }

        public void InsertAt(int index, AbstractSyntaxTreeNode node)
        {
            Children.Insert(index, node);
            UpdateMembers();
        }

        public void RemoveAt(int index)
        {
            if (index < 0 && index >= Children.Count)
                throw new ArgumentOutOfRangeException();
            Children.RemoveAt(index);
            UpdateMembers();
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is AbstractSyntaxTreeNode node && HashTag == node.HashTag;
        }

        public override string ToString()
        {
            return Value;
        }

        public override int GetHashCode()
        {
            throw new InvalidOperationException($"{nameof(AbstractSyntaxTreeNode)} does not have a consistent hash code.");
        }

        public static bool operator==(AbstractSyntaxTreeNode lhs, AbstractSyntaxTreeNode rhs)
        {
            if (lhs is null && rhs is null)
                return true;
            if (lhs is null) return false;

            return !(rhs is null) && lhs.Equals(rhs);
        }

        public static bool operator!=(AbstractSyntaxTreeNode lhs, AbstractSyntaxTreeNode rhs)
        {
            return !(lhs == rhs);
        }

        public void PreOrder(IAbstractSyntaxTreeVisitor visitor)
        {
            visitor.Visit(this);
            foreach (var node in Children)
            {
                node.PreOrder(visitor);
            }
        }

        public void PreOrder(IAbstractSyntaxTreeSearchVisitor visitor)
        {
            if (visitor.Found) return;

            visitor.Visit(this);
            var count = 0;

            while (!visitor.Found && count < Children.Count)
            {
                Children[count++].PreOrder(visitor);
            }
        }

        public void PostOrder(IAbstractSyntaxTreeVisitor visitor)
        {
            foreach (var node in Children)
            {
                node.PostOrder(visitor);
            }
            visitor.Visit(this);
        }

        public void PostOrder(IAbstractSyntaxTreeSearchVisitor visitor)
        {
            var count = 0;
            while (!visitor.Found && count < Children.Count)
            {
                Children[count++].PostOrder(visitor);
            }

            if (!visitor.Found)
            {
                visitor.Visit(this);
            }
        }

        public void BreadthFirst(IAbstractSyntaxTreeSearchVisitor visitor)
        {
            var queue = new Queue<AbstractSyntaxTreeNode>();
            queue.Enqueue(this);

            while (queue.Count != 0 && !visitor.Found)
            {
                var node = queue.Dequeue();

                visitor.Visit(node);

                foreach (var astNode in node.Children)
                {
                    queue.Enqueue(astNode);
                }
            }

            queue.Clear();
        }

        public void BreadthFirst(IAbstractSyntaxTreeVisitor visitor)
        {
            var queue = new Queue<AbstractSyntaxTreeNode>();
            queue.Enqueue(this);

            while (queue.Count != 0)
            {
                var node = queue.Dequeue();

                visitor.Visit(node);

                foreach (var astNode in node.Children)
                {
                    queue.Enqueue(astNode);
                }
            }
        }

    }
}
