using System;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace GaussAlgorithm;

public class Solver
{
	public double[] Solve(double[][] matrix, double[] freeMembers)
	{
        int row = matrix.GetLength(0);
        int col = matrix[0].Length;
        double multi1, multi2;
        double[] result = new double[col];
        
        for (int k = 0; k < row; k++)
        {
            if(k == col)
            {
                break;
            }
            if (Math.Abs(matrix[k][k]) <= 1e-6)
            {
                for (int i = k + 1; i < row; i++)
                {
                    if (Math.Abs(matrix[i][k]) > 1e-6)
                    {
                        for (int j = 0; j < col; j++)
                        {
                            (matrix[i][j], matrix[k][j]) = (matrix[k][j], matrix[i][j]);
                        }
                        (freeMembers[i], freeMembers[k]) = (freeMembers[k], freeMembers[i]);
                        break;
                    }
                }
            }

            for (int j = k + 1; j < row; j++)
            {
                multi1 = Math.Abs(matrix[k][k]) < 1e-6 ? 0 : matrix[j][k] / matrix[k][k];
                for (int i = k; i < col; i++)
                {
                    matrix[j][i] = matrix[j][i] - multi1 * matrix[k][i];
                }
                freeMembers[j] = freeMembers[j] - multi1 * freeMembers[k];
            }
        }

        for(int i = 0; i < row; i++)
        {
            if (Math.Abs(matrix[i].Sum()) < 1e-6 && Math.Abs(freeMembers[i]) > 1e-6)
            {
                throw new NoSolutionException("No Solution");
            }
        }

        for (int k = Math.Min(col - 1, row - 1); k >= 0; k--)
        {
            multi1 = 0;
            if(row == 1 && Math.Abs(matrix[k][k]) < 1e-6)
            {
                for (int j = k; j < col; j++)
                {
                    multi2 = matrix[k].Sum() * result[j];
                    multi1 += multi2;
                    result[j] = Math.Abs(matrix[k][j]) < 1e-6 ? 0 : (freeMembers[k] - multi1) / matrix[k][j];
                }
            }
            else
            {
                for (int j = k; j < Math.Min(col, row); j++)
                {
                    multi2 = matrix[k][j] * result[j];
                    multi1 += multi2;
                }
                result[k] = Math.Abs(matrix[k][k]) < 1e-6 ? 0 : (freeMembers[k] - multi1) / matrix[k][k];
            }
        }

        return result;
    }
}