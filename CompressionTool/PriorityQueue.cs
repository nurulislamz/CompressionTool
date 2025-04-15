namespace CompressionTool
{
    public interface IPriorityQueue
    {
        public void Push(HuffmanNode huffmanNode);
        public HuffmanNode? Pop();
        public HuffmanNode? Top();
        public void Heapify(List<HuffmanNode> nums);
        public int Count();
    }

    public class HuffmanNode
    {
        public char? Character { get; set; }
        public int Frequency { get; set; }
        public HuffmanNode? Left { get; set; }
        public HuffmanNode? Right { get; set; }

        public HuffmanNode(char? character, int frequency)
        {
            Character = character;
            Frequency = frequency;
            Left = null;
            Right = null;
        }

        public bool IsLeaf => Character.HasValue;

    }
    public class PriorityQueue : IPriorityQueue
    {
        public List<HuffmanNode> heap;

        public PriorityQueue()
        {
            heap = new List<HuffmanNode>();
            heap.Add(new HuffmanNode(null, 0));
        }

        public int Count() => heap.Count;
        public void Push(HuffmanNode newNode)
        {
            heap.Add(newNode);
            BubbleUp(heap.Count - 1);
        }

        public HuffmanNode? Pop()
        {
            if (heap.Count == 1)
            {
                throw new Exception("Empty heap");
            }

            if (heap.Count == 2)
            {
                HuffmanNode pop = heap[1];
                heap.RemoveAt(1);
                return pop;
            }

            HuffmanNode root = heap[1];
            heap[1] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            if (heap.Count > 1)
            {
                BubbleDown(1);
            }
            return root;
        }

        public HuffmanNode? Top()
        {
            return heap.Count > 1 ? heap[1] : null;
        }

        public void Heapify(List<HuffmanNode> nums)
        {
            heap = new List<HuffmanNode>();
            heap.Add(new HuffmanNode('d', 0));
            heap.AddRange(nums);

            for (int i = heap.Count / 2; i >= 1; i--)
            {
                BubbleDown(i);
            }
        }

        public void BubbleUp(int index)
        {
            if (index <= 1)
            {
                return;
            }

            int parent = index / 2;

            while (index > 1 && heap[parent].Frequency > heap[index].Frequency)
            {
                Swap(parent, index);
                index = parent;
                parent = index / 2;
            }
        }

        public void BubbleDown(int index)
        {
            int heapLength = heap.Count - 1;

            while (true)
            {
                int smallest = index;
                int left = 2 * index;
                int right = 2 * index + 1;

                if (left <= heapLength && heap[left].Frequency < heap[smallest].Frequency)
                {
                    smallest = left;
                }

                if (right <= heapLength && heap[right].Frequency < heap[smallest].Frequency)
                {
                    smallest = right;
                }

                if (smallest != index)
                {
                    Swap(index, smallest);
                    index = smallest;
                }
                else
                {
                    break;
                }
            }
        }

        public void Swap(int i, int j)
        {
            HuffmanNode tmp = heap[i];
            heap[i] = heap[j];
            heap[j] = tmp;
        }
    }
}