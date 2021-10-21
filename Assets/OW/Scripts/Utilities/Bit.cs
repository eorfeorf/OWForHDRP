namespace OW.Scripts.Utilities
{
    public class Bit
    {
        private int bitFlag;
    
        public void Add(int shift)
        {
            bitFlag |= (1 << shift);
        }

        public void Set(int shift)
        {
            bitFlag = (1 << shift);
        }

        public bool Is(int shift)
        {
            return bitFlag == (1 << shift);
        }
        
        public bool Has(int shift)
        {
            return (bitFlag & (1 << shift)) == (1 << shift);
        }

        public void Del(int shift)
        {    
            bitFlag &= ~(1 << shift);
        }

        public void Reset()
        {
            bitFlag = 0;
        }
    }
}
