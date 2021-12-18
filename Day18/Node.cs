using Newtonsoft.Json.Linq;

internal class Node
{
    public Node? Left { get; set; }

    public Node? Right { get; set; }

    public int Value { get; set; }

    public Node Parent { get; set; } 

    public bool IsLeaf { get => Left == null && Right == null; }

    public Node(int value, Node parent = null)
    {
        Value = value;
        Parent = parent;
    }

    public Node(Node left, Node right, Node parent = null)
    {
        Left = left;
        Left.Parent = this;

        Right = right;
        Right.Parent = this;

        Parent = parent;
    }

    public Node(JToken token, Node parent = null)
    {
        if (parent != null)
        {
            this.Parent = parent;
        }
        if (token.Type == JTokenType.Integer)
        {
            this.Value = token.ToObject<int>();
        }
        else if (token.Type == JTokenType.Array)
        {
            var items = token.ToArray();
            this.Left = new Node(items[0], this);
            this.Right = new Node(items[1], this);
        }
        else
        {
            throw new ArgumentException($"Jtoken type {token.Type} is not a valid type");
        }
    }

    public Node LeftMost()
    {
        if (this.Left != null)
        {
            return this.Left.LeftMost();
        }
        else
        {
            return this;
        }
    }

    public Node RightMost()
    {
        if (this.Right != null)
        {
            return this.Right.RightMost();
        }
        else
        {
            return this;
        }
    }

    public int Magnitude { 
        get 
        {
            if (!IsLeaf)
            {
                return 3 * this.Left.Magnitude + 2 * this.Right.Magnitude;
            }
            else
            {
                return this.Value;
            }
        }
    }

    public override string ToString()
    {
        if (!IsLeaf)
        {
            return $"[{this.Left},{this.Right}]";
        }
        else
        {
            return this.Value.ToString();
        }
    }

    public Node? FindExplosionNode(int moreDepth)
    {
        if (this.IsLeaf)
        {
            return null;
        }

        if (moreDepth <= 0 && Left.IsLeaf && Right.IsLeaf)
        {
            return this;
        }

        return 
            Left?.FindExplosionNode(moreDepth - 1) ??
            Right?.FindExplosionNode(moreDepth - 1);
    }

    public Node? FindSplitNode()
    {
        if (this.IsLeaf && this.Value < 10)
        {
            return null;
        }
        if (this.IsLeaf && this.Value >= 10)
        {
            return this;
        }
        return Left?.FindSplitNode() ?? Right?.FindSplitNode();
    }

    public Node ImmediatelyLeft()
    {
        if (Parent == null)
        {
            return null;
        }
        if (Parent.Right == this) // We are on the right
        {
            return Parent.Left.RightMost();
        }
        else
        {
            return this.Parent.ImmediatelyLeft();
        }
    }

    public Node ImmediatelyRight()
    {
        if (Parent == null)
        {
            return null;
        }
        if (Parent.Left == this) // We are on the right
        {
            return Parent.Right.LeftMost();
        }
        else
        {
            return this.Parent.ImmediatelyRight();
        }
    }

    public void Explode()
    {
        if (!this.Left.IsLeaf || !this.Right.IsLeaf)
        {
            throw new ArgumentException("We are trying to explode a node that is not a pair of leaf nodes");
        }

        Node immediatelyRight = this.ImmediatelyRight();
        if (immediatelyRight != null)
        {
            immediatelyRight.Value += this.Right.Value;
        }

        Node immediatelyLeft = this.ImmediatelyLeft();
        if (immediatelyLeft != null)
        {
            immediatelyLeft.Value += this.Left.Value;
        }

        // At the end, reset everything and transfomr this into a 0 leaf node.
        this.Left = null;
        this.Right = null;
        this.Value = 0;
    }

    public void Split()
    {
        if (!this.IsLeaf || this.Value < 10)
        {
            throw new ArgumentException("This Node to split does not have a value >= 10");
        }

        this.Left = new Node(this.Value / 2, this);
        this.Right = new Node((this.Value + 1) / 2, this);
        this.Value = 0;
    }
}

