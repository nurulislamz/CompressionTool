namespace CompressionTool
{
    public interface IPriorityQueue
    {
        public void Push(char? character, int val);
        public HeapNode? Pop();
        public HeapNode? Top();
        public void Heapify(List<HeapNode> nums);
    }
    
    public class HeapNode
    {
        public char? Character { get; set; }
        public int Frequency { get; set; }

        public HeapNode(char? character, int frequency)
        {
            Character = character;
            Frequency = frequency;
        }
    }

    public class PriorityQueue : IPriorityQueue
    {
        public List<HeapNode> heap;

        public PriorityQueue()
        {
            heap = new List<HeapNode>();
            heap.Add(new HeapNode(null, 0));
        }

        public void Push(char? character, int val)
        {
            HeapNode newNode = new HeapNode(character, val);
            heap.Add(newNode);
            BubbleUp(heap.Count - 1);
        }

        public HeapNode? Pop()
        {
            if (heap.Count == 1)
            {
                return null;
            }

            if (heap.Count == 2)
            {
                HeapNode pop = heap[1];
                heap.RemoveAt(1);
                return pop;
            }

            HeapNode root = heap[1];
            heap[1] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            if (heap.Count > 1)
            {
                BubbleDown(1);
            }
            return root;
        }

        public HeapNode? Top()
        {
            return heap.Count > 1 ? heap[1] : null;
        }

        public void Heapify(List<HeapNode> nums)
        {
            heap = new List<HeapNode>();
            heap.Add(new HeapNode('d', 0));
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
            HeapNode tmp = heap[i];
            heap[i] = heap[j];
            heap[j] = tmp;
        }
    }
}