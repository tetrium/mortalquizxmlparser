
using AxiomCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


    public class ByteData
    {
        public int bufferSize { get; set; }
        public byte[] buffer { get; set; }
    }



    public static class Utils
    {
        public static int BLOCK_SIZE = 4096;

        public static int GetRandom(int min,int max) {
            Random r = new Random();
            int rInt = r.Next(min,max); //for ints
           // int range = max;
            //double rDouble = r.NextDouble() * range; //for doubles
             return rInt;
        }

   
    public static Dictionary<string, List<string>> Csv2Dictionary(string texto,List<string> mandatoryHeaders)
    {
        Dictionary<string, List<string>> diccionario = new Dictionary<string, List<string>>();
        Dictionary<int, string> meta = new Dictionary<int, string>();
        List<string> listaValores = new List<string>();
        string nombreColumna = "";
        texto = texto.Replace("\r", "");
        string[] renglones = texto.Split('\n');
        int nr = 0;
        int nc = 0;
        foreach (string renglon in renglones)
        {
            string[] tokens = renglon.Split(';');
            nc = 0;
            foreach (var token in tokens)
            {

                if (nr == 0)
                {

                    if (!diccionario.ContainsKey(token))
                    {

                        diccionario.Add(token.Trim(), new List<string>());
                        meta.Add(nc, token.Trim());

                    }

                }
                else
                {
                    try
                    {
                        if (!token.Equals(""))
                        {
                            nombreColumna = meta[nc];
                            listaValores = diccionario[nombreColumna];
                            listaValores.Add(token);
                            diccionario[nombreColumna] = listaValores;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(" el renglon " + nr + " sobrepasa el numero de columnas definidas por el header");
                    }
                }
                nc++;
            }
            nr++;
        }
        if (mandatoryHeaders != null)
        {
            foreach (string header in mandatoryHeaders)
            {
                if (!diccionario.ContainsKey(header)) {
                    throw new Exception(" \""+header+"\" "+StringPool.instance.GetString("CsvMandatoryHeaderIsUndefined"));

                }
            }
        }

        return diccionario;
    }
    public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }



        /// <summary>
        /// Convierte un entero decimal a una chain en hexadecimal
        /// </summary>
        /// <param name="inDec">Entero decimal a convertir</param>
        /// <returns></returns>
        public static string DecToHex(int inDec)
        {
            string stHex = "";
            try
            {
                stHex = inDec.ToString("X");
            }
            catch (Exception )
            {

            throw;
            }
            return stHex;
        }

        /// <summary>
        /// Convierte una chain hexadecimal a una chain en formato ascii
        /// </summary>
        /// <param name="hexString">Chain hexadecimal a convertir</param>
        /// <returns></returns>
        public static string HexToAsc(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            String ascii = "";
            String hs = "";
            int value = 0;
            string stringValue = "";
            char charValue;
            //log.Print(5, "Hex: " + hexString);
            try
            {
                //log.Print(5, "Long: " + hexString.Length);
                for (int i = 0; i < hexString.Length; i += 2)
                {
                    hs = hexString.Substring(i, 2);
                    value = Convert.ToInt32(hs, 16);
                    stringValue = Char.ConvertFromUtf32(value);
                    charValue = (char)value;
                    //log.Print(5, "ascii[i]: " + charValue);
                    ascii += charValue;
                }
                //log.Print(5, "Ascii: " + ascii);
            }
            catch(Exception )
            {
            throw;
            }
            return ascii;
        }


        /// <summary>
        /// Convierte una chain hexadecimal a una chain en formato decimal
        /// </summary>
        /// <param name="hexNumber">Chain hexadecimal a convertir</param>
        /// <returns></returns>
        public static string HexToDec(string hexNumber)
        {
            string decNumber = "";
            try
            {
                hexNumber = hexNumber.Replace("x", string.Empty);
                long result = 0;
                long.TryParse(hexNumber, System.Globalization.NumberStyles.HexNumber, null, out result);
                decNumber = result.ToString();
            }
            catch (Exception )
            {
            throw;
            }
            return decNumber;
        }
       

        /// <summary>
        /// Convierte una chain en ascii a una chain en hexadecimal
        /// </summary>
        /// <param name="stAsc">Chain a convertir</param>
        /// <returns></returns>
        public static string AscToHex(string stAsc)
        {
           
            char[] array = stAsc.ToCharArray();
            string final = "";
            foreach (var i in array)
            {
                string hex = String.Format("{0:X}", Convert.ToInt32(i));
                final += hex.Insert(0, "");       
            }
            return final = final.TrimEnd();
      }
        
       
      
        /// <summary>
        /// Convierte un arreglo de bytes a una chain:
        /// (In = {0x00, 0x01, 0x02} )
        /// (Out = "000102")
        /// </summary>
        /// <param name="bytes">Arreglo de bytes a convertir</param>
        /// <returns>string </returns>
        public static string Bytes2String(byte[] bytes) 
        {
            int bytesPerLine = bytes.Length;
            if (bytes == null) return "<null>";
            int bytesLength = bytes.Length;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                  8                   // 8 caracteres 
                + 3;                  // 3 espacios

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';

                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];

                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);

            }

            return result.ToString().Trim().Replace(" ", string.Empty);
        }
        /// <summary>
        /// Convierte una chain de caracteres hexadecimales a un arreglo de bytes en representación hexadecimal.
        /// (In = "012")
        /// (Out = {0x30, 0x31, 0x32})
        /// </summary>
        /// <param name="stAsc">Chain a convertir</param>
        /// <returns></returns>
        public static Byte[] Asc2Bytes(String stAsc) // ENTRADA="001"  SALIDA=0x30,0x30,0x31
        {
            return Encoding.ASCII.GetBytes(stAsc);
        }


    public static string GetPOSEntryMode(PosEntryMode posEntryMode,AcquirerId acquirerId) // ENTRADA="001"  SALIDA=0x30,0x30,0x31
    {
        byte[] mode=String2Bytes("00");
        if (acquirerId==AcquirerId.Prosa) {
            if (posEntryMode == PosEntryMode.Unknown) {
                return "00";
            }
            if (posEntryMode == PosEntryMode.Manual)
            {
                return "01";
            }
            if (posEntryMode == PosEntryMode.IntegratedCircuitRead)
            {
                return "05";
            }
            if (posEntryMode == PosEntryMode.ContactlessChip)
            {
                return "07";
            }
            if (posEntryMode == PosEntryMode.FallBack)
            {
                return "80";
            }
            if (posEntryMode == PosEntryMode.ElectronicCommerce)
            {
                return "81";
            }
            if (posEntryMode == PosEntryMode.MagneticStripe)
            {
                return "90";
            }
            if (posEntryMode == PosEntryMode.Contactless)
            {
                return "91";
            }

        }

        if (acquirerId == AcquirerId.BBVA)
        {
            if (posEntryMode == PosEntryMode.Unknown)
            {
                return "00";
            }
            if (posEntryMode == PosEntryMode.Manual)
            {
                return "01";
            }
            if (posEntryMode == PosEntryMode.IntegratedCircuitRead)
            {
                return "05";
            }
            if (posEntryMode == PosEntryMode.ContactlessChip)
            {
                return "07";
            }
            if (posEntryMode == PosEntryMode.FallBack)
            {
                return "80";
            }
            if (posEntryMode == PosEntryMode.ElectronicCommerce)
            {
                return "81";
            }
            if (posEntryMode == PosEntryMode.MagneticStripe)
            {
                return "90";
            }
            if (posEntryMode == PosEntryMode.Contactless)
            {
                return "91";
            }


        }


        return "00";
    }


    public static byte[] IntHex2Hex(int d) {

            string hexValue = d.ToString();
            return String2Bytes(hexValue);
        }
    public static byte[] DecimalHex2Bytes(decimal d)
    {

        string hexValue = d.ToString();
        return Asc2Bytes(hexValue);
    }

    public static byte[] DateTime2Bytes(DateTime d)
    {
        string hexValue = d.Ticks.ToString("X2");


       // string hexValue = d.ToString();
        return String2Bytes(hexValue);
    }

    public static byte[] Int2Hex(int d)
    {

        string hexValue = d.ToString("X");
        return String2Bytes(hexValue);
    }

    /// <summary>
    /// Convierte una chain en formato hexadecimal a un arreglo de bytes.
    /// (In = "012")
    /// (Out = {0x00, 0x01, 0x02})
    /// </summary>
    /// <param name="stHex">Chain a convertir</param>
    /// <returns></returns>
    public static Byte[] String2Bytes(string stHex) // ENTRADA="001"  SALIDA=0x00,0x00,0x01
        {
            Byte[] bytes = new Byte[(int)(stHex.Length * 0.5f)];
            int i = 0;

            while (stHex.Length > 0)
            {
                bytes[i] = (byte)System.Convert.ToChar(System.Convert.ToUInt32(stHex.Substring(0, 2), 16));
                stHex = stHex.Substring(2, stHex.Length - 2);
                i++;
            }
            return bytes;
        }
        /// <summary>
        /// Convierte un arreglo de bytes a una chain en representación ascii.
        /// (In = {0x30, 0x31, 0x32}))
        /// (Out = "012")
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String Bytes2Asc(Byte[] byBytes)  // ENTRADA=0x30,0x30,0x31  SALIDA="001"
        {
            return System.Text.Encoding.ASCII.GetString(byBytes); ;
        }

       
        public static string ToHex(string input)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (char c in input)
                    sb.AppendFormat("{0:X2}", (int)c);
            }
            catch (Exception )
            {
            throw;
            }
            return sb.ToString().Trim();
        }


        public static string ToDec(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            string ascii = string.Empty;
            try
            {
                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;
                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;
                }
            }
            catch(Exception )
            {
            throw;
            }
            return ascii;
        }

        /// <summary>
        /// Convierte del formato de fecha "dd/mm/aaaa hh:mm:ss" a "aaaa-mm-dd hh:mm:ss"
        /// </summary>
        /// <param name="stDat"></param>
        /// <returns></returns>
        public static bool TransDate(ref string stDat)
        {                                       
            bool st = false;
            try
            {
                string Day = stDat.Substring(0, 2);
                string Month = stDat.Substring(3, 2);
                string Year = stDat.Substring(6, 4);
                stDat = Year + "/" + Month + "/" + Day + " " + stDat.Substring(11, 7) + "0";
                st = true;
            }
            catch (Exception )
            {
             throw;
               // st = false;
            }
            return st;
        }


       
        public static void HexStringToAscString(ref String hexString)
        {
            StringBuilder sb = new StringBuilder();
          
            for (int i = 0; i < hexString.Length; i = i+2)
            {
                String hs = hexString.Substring(i, i + 2);
                System.Convert.ToChar(System.Convert.ToUInt32(hexString.Substring(0, 2), 16)).ToString();
            }

            hexString = sb.ToString();
           
        }


        /*public static Byte[] getByteMessage(List<ByteData> bytes, int TotalSize)
        {
            //log.Print("Receiving data...");

            Byte[] StreamData = new Byte[TotalSize];
            int StreamIndex = 0;

            foreach (var readbytes in bytes)
            {
                int currentSize = readbytes.size;
                Array.Copy(readbytes.StreamData, 0, StreamData, StreamIndex, currentSize);
                StreamIndex += readbytes.StreamData.Length;
            }

            return StreamData;
        }*/



    }

