using System;
using System.Collections;
using System.Collections.Generic;


namespace MyDictionary
{
    /// <summary>
    /// IDictionary interface implementation with avl tree
    /// </summary>
    /// <typeparam name="TKey">the type of keys</typeparam>
    /// <typeparam name="TValue">the type of values</typeparam>
    class AvlDictionary<TKey, TValue> : IDictionary<TKey, TValue> 
    where TKey : IComparable<TKey>
    {
        /// <summary>
        /// returns the enumerator
        /// </summary>
        /// <returns>the enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// iterates through the tree
        /// </summary>
        /// <returns>returns the iterator of the collection of nodes</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var collection = new List<KeyValuePair<TKey, TValue>>();
            Stack st = new Stack();
            Node current = this.root;
            bool flag = true;
            while (flag)
            {
                while (current != null)
                {
                    st.Push(current);
                    current = current.left;
                }

                if (st.Count == 0)
                    flag = false;
                else
                {
                    current = (Node)st.Pop();
                    collection.Add(current);
                    current = current.right;
                }
            }
            return collection.GetEnumerator();
        }

        /// <summary>
        /// add new item by key value pair
        /// </summary>
        /// <param name="item">the item to be added</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Insert(item.Key,item.Value);
        }

        /// <summary>
        /// clear tree
        /// </summary>
        public void Clear()
        {
            this.root = null;
        }

        /// <summary>
        /// check if the item by this key and value is set
        /// </summary>
        /// <param name="item">the item to be checked</param>
        /// <returns>true if contains false otherwise</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Find(item.Key);
        }

        /// <summary>
        /// copy the the nodes of tree starting form arrayIndex
        /// </summary>
        /// <param name="array">he array where to copy</param>
        /// <param name="arrayIndex">index from where to copy</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            List<Node> collection = new List<Node>();
            Node r = this.root;
            Stack<Node> st = new Stack<Node>();
            bool b = true;
            while (b)
            {
                while (r != null)
                {
                    st.Push(r);
                    r = r.left;
                }
                if (st.Count == 0)
                {
                    b = false;
                }
                else
                {
                    r = st.Pop();
                    collection.Add(r);
                    r = r.right;
                }
            }
             
            for (int i = 0; i < collection.Count && arrayIndex < array.Length; i++ , arrayIndex++ )
            {
                array[arrayIndex] = new KeyValuePair<TKey, TValue>(collection[i].key, collection[i].data);
            }
        }

        /// <summary>
        /// Remove item
        /// </summary>
        /// <param name="item">the irem to be removed</param>
        /// <returns>true if removed false otherwise</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Delete(item.Key);
        }

        /// <summary>
        /// changes the value with the given key
        /// </summary>
        /// <param name="key">the given key</param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                TryGetValue(key, out value);
                return value;
            }
            set { Replace(key, value); }
        }

        /// <summary>
        /// remove node with given key
        /// </summary>
        /// <param name="key">the given key</param>
        /// <returns>true if is deleted false otherwise</returns>
        public bool Remove(TKey key)
        {
            return Delete(key);
        }

        /// <summary>
        /// add new node with given key and value
        /// </summary>
        /// <param name="key">the given key</param>
        /// <param name="data">the given value</param>
        public void Add(TKey key, TValue data)
        {
            Insert(key, data);
        }

        /// <summary>
        /// check if contains node with the kay
        /// </summary>
        /// <param name="key">the given key</param>
        /// <returns>true if contains false otherwise</returns>
        public bool ContainsKey(TKey key)
        {
            return Find(key);
        }

        /// <summary>
        /// try to get the value with given key
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (Find(key, root) != null)
            {
                value = Find(key, root).data;
                return true;
            }
    
            value = default(TValue);
            return false;
            
        }

        public class Node
        {
            public TValue data;
            public TKey key;
            public Node left;
            public Node right;
            public Node(TKey key= default(TKey), TValue data = default(TValue))
            {
                this.data = data;
                this.key = key;
                this.left = null;
                this.right = null;
            }

            public static implicit operator KeyValuePair<TKey, TValue>(Node node)
            {
                return new KeyValuePair<TKey, TValue>(node.key,node.data);
            }
        }

        /// <summary>
        /// collection of all keys
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                return InorderTraversalKeys();
            }

        }

        public ICollection<TValue> Values
        {
            get { return InorderTraversalValues(); }
        }

        /// <summary>
        /// collects all keys by inorder travers of tree
        /// </summary>
        /// <returns>reurn list of keys</returns>
        private ICollection<TKey> InorderTraversalKeys()
        {
            ICollection<TKey> collection = new List<TKey>();
            Stack st = new Stack();
            Node r = this.root;
            bool flag = true;
            while (flag)
            {
                while (r != null)
                {
                    st.Push(r);
                    r = r.left;
                }

                if (st.Count == 0)
                    flag = false;
                else
                {
                    r = (Node)st.Pop();
                    collection.Add(r.key);
                    r = r.right;
                }
            }
            return collection;
        }

        /// <summary>
        /// collects all keys by inorder travers of tree
        /// </summary>
        /// <returns>list of values</returns>
        private ICollection<TValue> InorderTraversalValues()
        {
            ICollection<TValue> collection = new List<TValue>();
            Stack st = new Stack();
            Node r = this.root;
            bool flag = true;
            while (flag)
            {
                while (r != null)
                {
                    st.Push(r);
                    r = r.left;
                }

                if (st.Count == 0)
                    flag = false;
                else
                {
                    r = (Node)st.Pop();
                    collection.Add(r.data);
                    r = r.right;
                }
            }
            return collection;
        }

        /// <summary>
        /// the count
        /// </summary>
        public int Count
        {
            get
            {
                var count = 0;
                Node r = this.root;
                Stack<Node> st = new Stack<Node>();
                bool b = true;
                while (b)
                {
                    while (r != null)
                    {
                        st.Push(r);
                        r = r.left;
                    }
                    if (st.Count == 0)
                    {
                        b = false;
                    }
                    else
                    {
                        r = st.Pop();
                        count++;
                        r = r.right;
                    }
                }

                return count;
            }
        }

        public bool IsReadOnly => throw new NotImplementedException();

        int ICollection<KeyValuePair<TKey, TValue>>.Count => throw new NotImplementedException();

        /// <summary>
        /// the root
        /// </summary>
        private Node root;
        public AvlDictionary(TKey key, TValue value)
        {
            this.root.key = key;
            this.root.data = value;
        }
            public void Insert(TKey key, TValue data)
            {
                Node newItem = new Node(key,data);
                if (root == null)
                    root = newItem;
                else
                     root = RecursiveInsert(root, newItem);
              
            }

    
        private Node RecursiveInsert(Node current, Node newItem)
            {
              
                if (newItem.key.CompareTo(current.key) < 0)
                {
                    current.left = RecursiveInsert(current.left, newItem);
                    current = BalanceTree(current);
                }
                else if (newItem.key.CompareTo(current.key) > 0)
                {
                    current.right = RecursiveInsert(current.right, newItem);
                    current = BalanceTree(current);
                }
                return current;
            }
            private Node BalanceTree(Node current)
            {
                int balancFact = TreeBalanceFactor(current);
                if (balancFact > 1)
                {
                    if (TreeBalanceFactor(current.left) > 0)
                    {
                        current = RotateLL(current);
                    }
                    else
                    {
                        current = RotateLR(current);
                    }
                }
                else if (balancFact < -1)
                {
                    if (TreeBalanceFactor(current.right) > 0)
                    {
                        current = RotateRL(current);
                    }
                    else
                    {
                        current = RotateRR(current);
                    }
                }
                return current;
            }
            public bool Delete(TKey target)
            {
                Node res = Delete(root, target);
                if (res == null)
                    return false;
                    return true;
                
            }
            private Node Delete(Node current, TKey target)
            {
                Node parent;
                if (current == null)
                    return null;
                else
                {
                    //left subtree
                    if (target.CompareTo(current.key) < 0)
                    {
                        current.left = Delete(current.left, target);
                        if (TreeBalanceFactor(current) == -2)
                        {
                            if (TreeBalanceFactor(current.right) <= 0)
                            {
                                current = RotateRR(current);
                            }
                            else
                            {
                                current = RotateRL(current);
                            }
                        }
                    }
                    //right subtree
                    else if (target.CompareTo(current.key) > 0)
                    {
                        current.right = Delete(current.right, target);
                        if (TreeBalanceFactor(current) == 2)
                        {
                            if (TreeBalanceFactor(current.left) >= 0)
                            {
                                current = RotateLL(current);
                            }
                            else
                            {
                                current = RotateLR(current);
                            }
                        }
                    }
                    //if target is found
                    else
                    {
                        if (current.right != null)
                        {
                            //delete its inorder successor
                            parent = current.right;
                            while (parent.left != null)
                            {
                                parent = parent.left;
                            }
                            current.data = parent.data;
                            current.key = parent.key;
                            current.right = Delete(current.right, parent.key);
                            if (TreeBalanceFactor(current) == 2)//rebalancing
                            {
                                if (TreeBalanceFactor(current.left) >= 0)
                                {
                                    current = RotateLL(current);
                                }
                                else { current = RotateLR(current); }
                            }
                        }
                        else
                        {   //if current.left != null
                            return current.left;
                        }
                    }
                }
                return current;
            }
            public bool Find(TKey key)
            {
                if (Find(key, root).key.CompareTo(key) == 0)
                {
                    Console.WriteLine("{0} was found!", key);
                    return true;
                }
                else
                {
                    Console.WriteLine("Nothing found!");
                    return false;
            }
            }
            private Node Find(TKey target, Node current)
            {

                if (target.CompareTo(current.key) < 0)
                {
                    if (target.CompareTo(current.key) == 0)
                    {
                        return current;
                    }
                    else
                        return Find(target, current.left);
                }
                else
                {
                    if (target.CompareTo(current.key) == 0)
                    {
                        return current;
                    }
                    else
                        return Find(target, current.right);
                }

            }
           
            private int Max(int l, int r)
            {
                return l > r ? l : r;
            }
           
            private int TreeBalanceFactor(Node current)
            {
                int l = GetHeight(current.left);
                int r = GetHeight(current.right);
                int balancFact = l - r;
                return balancFact;
            }

             private int GetHeight(Node current)
             {
                var height = 0;
                if (current != null)
                {
                    var l = GetHeight(current.left);
                    var r = GetHeight(current.right);
                    height = Max(l, r) + 1;
                }
                return height;
             }

            private Node RotateRR(Node parent)
            {
                Node node = parent.right;
                parent.right = node.left;
                node.left = parent;
                return node;
            }
            private Node RotateLL(Node parent)
            {
                Node node = parent.left;
                parent.left = node.right;
                node.right = parent;
                return node;
            }
            private Node RotateLR(Node parent)
            {
                Node node = parent.left;
                parent.left = RotateRR(node);
                return RotateLL(parent);
            }
            private Node RotateRL(Node parent)
            {
                Node node = parent.right;
                parent.right = RotateLL(node);
                return RotateRR(parent);
            }

        private void Replace(TKey key, TValue value)
        {
            Find(key, this.root).data = value;
        }
    }
    }
