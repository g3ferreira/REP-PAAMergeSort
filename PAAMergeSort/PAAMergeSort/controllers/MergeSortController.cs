using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAAMergeSort.controllers
{
    public class MergeSortController
    {
        public int[] fazerMerge(int[] numeros, int elementoEsquerda, int elementoMeio, int elementoDireita)
        {
            int[] temp = new int[numeros.Length];
            int i, left_end, num_elements, tmp_pos;

            left_end = (elementoMeio - 1);
            tmp_pos = elementoEsquerda;
            num_elements = (elementoDireita - elementoEsquerda + 1);

            while ((elementoEsquerda <= left_end) && (elementoMeio <= elementoDireita))
            {
                if (numeros[elementoEsquerda] <= numeros[elementoMeio])
                    temp[tmp_pos++] = numeros[elementoEsquerda++];
                else
                    temp[tmp_pos++] = numeros[elementoMeio++];
            }

            while (elementoEsquerda <= left_end)
                temp[tmp_pos++] = numeros[elementoEsquerda++];

            while (elementoMeio <= elementoDireita)
                temp[tmp_pos++] = numeros[elementoMeio++];

            for (i = 0; i < num_elements; i++)
            {
                numeros[elementoDireita] = temp[elementoDireita];
                elementoDireita--;
            }

            return numeros;
        }

        public void mergeSortRecursivo(int[] numeros, int elementoEsquerda, int elementoDireita)
        {
            int[] numerob = new int[numeros.Length];
            int elementoMeio;

            if (elementoDireita > elementoEsquerda)
            {
                elementoMeio = (elementoDireita + elementoEsquerda) / 2;
                mergeSortRecursivo(numeros, elementoEsquerda, elementoMeio);
                mergeSortRecursivo(numeros, (elementoMeio + 1), elementoDireita);

                numerob = fazerMerge(numeros, elementoEsquerda, (elementoMeio + 1), elementoDireita);
            }
        }




    }
}
