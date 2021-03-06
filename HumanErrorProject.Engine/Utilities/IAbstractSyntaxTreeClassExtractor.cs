﻿using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;

namespace HumanErrorProject.Engine.Utilities
{
    public interface IAbstractSyntaxTreeClassExtractor
    {
        AbstractSyntaxTreeNode Extract(AbstractSyntaxTreeNode root, string name);
        AbstractSyntaxTreeNode ExtractOrDefault(AbstractSyntaxTreeNode root, string name);
    }
}
