namespace PQueue
{
    public class Node
    {
        public char Character { get; set; }
        public int Frequency { get; set; }

        public Node(char character, int frequency)
        {
            Character = character;
            Frequency = frequency;
        }
    }

    public class PriorityQueue
    {
        List<Node> heap;

        public PriorityQueue()
        {
            heap = new List<Node>();
            heap.Add(new Node('d', 0));
        }

        public void Push(Node val)
        {
            heap.Add(val);
            BubbleUp(heap.Count - 1);
        }

        public Node? Pop()
        {
            if (heap.Count == 1)
            {
                return null;
            }

            if (heap.Count == 2)
            {
                Node pop = heap[1];
                heap.RemoveAt(1);
                return pop;
            }

            Node root = heap[1];
            heap[1] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            if (heap.Count > 1)
            {
                BubbleDown(1);
            }
            return root;
        }

        public Node? Top()
        {
            return heap.Count > 1 ? heap[1] : null;
        }

        public void Heapify(List<Node> nums)
        {
            heap = new List<Node>();
            heap.Add(new Node('d', 0));
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
            Node tmp = heap[i];
            heap[i] = heap[j];
            heap[j] = tmp;
        }
    }
}