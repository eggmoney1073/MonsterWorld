using UnityEngine;
using System.Text;

public class PlayerWeaponTable : TableBase
{
    public enum Index
    {
        Index,
        Type,
        Damage_Scale,

        count
    }

    public override void Load(string jsonData)
    {
        // Add 하면 됨
        string tableData = GetInsideText(GetInsideText(jsonData));

        string[] indexDatas = tableData.Split("},{");

        for (int i = 0; i < indexDatas.Length; i++)
        {
            string[] columDatas = GetInsideText(indexDatas[i], '\"', '\"').Split("\",\"");
            for (int j = 0; j < columDatas.Length; j++)
            {
                string[] colNVal = columDatas[j].Split("\":\"");

                if (colNVal[1].Contains("\\u"))
                {
                    StringBuilder stringbuilder = new StringBuilder();

                    // \ 로 나누고
                    string[] words = colNVal[1].Split("\\");

                    for (int k = 0; k < words.Length; k++)
                    {
                        if (words[k] != string.Empty && words[k].Contains('u'))
                        {
                            int wordLength = words[k].Length;
                            words[k] = words[k].Substring(1, wordLength - 1);
                            wordLength = words[k].Length;

                            if (wordLength > 4)
                            {
                                StringBuilder sb = new StringBuilder();

                                // 앞에 4자리만 가져온다.
                                string word = words[k].Substring(0, 4);
                                string remain = words[k].Substring(4, wordLength - 4);

                                char w = (char)int.Parse(word, System.Globalization.NumberStyles.AllowHexSpecifier);

                                sb.Append(w);
                                sb.Append(remain);

                                words[k] = sb.ToString();
                            }
                            else if (wordLength == 4)
                            {
                                char w = (char)int.Parse(words[k], System.Globalization.NumberStyles.AllowHexSpecifier);
                                words[k] = w.ToString();
                            }
                        }
                        stringbuilder.Append(words[k]);
                    }
                    colNVal[1] = stringbuilder.ToString();
                    stringbuilder.Clear();
                }

                Add((i + 1).ToString(), colNVal[0], colNVal[1]);

                //Debug.LogFormat("[Index:{0}, Column:{1}, value:{2}]", (i + 1).ToString(), colNVal[0], colNVal[1]);
            }
        }
    }

    string GetInsideText(string txt, char splitChar1 = '{', char splitChar2 = '}')
    {
        int first = txt.IndexOf(splitChar1);
        int last = txt.LastIndexOf(splitChar2);

        string result = txt.Substring(first + 1, last - first - 1);

        return result;
    }
}
