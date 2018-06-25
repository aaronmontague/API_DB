using System;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using API_DB.Controllers;
using System.Linq;
using System.Web;
using System.Text;

namespace API_DB.Classes
{
    public class generalDB
    {
        public string str = @"Data Source=TRYNUMBER3\SQLEXPRESS;Initial Catalog=test2;Integrated Security=True";
        public List<Person> getAllPersons()
        {
            SqlConnection con = new SqlConnection(str);

            SqlCommand cmdGetAll = new SqlCommand();

            SqlDataReader readAll;

            cmdGetAll.CommandText = "SELECT * FROM person_info";
            cmdGetAll.CommandType = CommandType.Text;
            cmdGetAll.Connection = con;

            try
            {
                con.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            readAll = cmdGetAll.ExecuteReader();

            List<Person> listOfAll = new List<Person>();

            while (readAll.Read())
            {
                //Object[] values = new Object[readAll.FieldCount];
                //int fieldCount = readAll.GetValues(values);

                //for (int i = 0; i < fieldCount; i++)
                //{
                //    //needs better looking JSON
                //    listOfAll += " " + readAll.GetName(i) + " : " + values[i] + ",";
                //}
                Person tempPerson = new Person(
                    readAll.GetInt32(0),
                    readAll.GetString(1),
                    readAll.GetString(2),
                    readAll.GetString(3),
                    readAll.GetInt32(4),
                    readAll.GetString(5),
                    readAll.GetString(6),
                    readAll.GetInt32(7)
                    );
                listOfAll.Add(tempPerson);
            }

            readAll.Close();

            return listOfAll;
        }

        public Person getSinglePersonById(int personId)
        {
            Person tempPerson = new Person();

            //check to see if the id is in the database
            SqlConnection checkCon = new SqlConnection(str);
            SqlConnection con = new SqlConnection(str);

            SqlCommand cmdCheckPersonExists = new SqlCommand();
            SqlCommand cmdGetSingleById = new SqlCommand();

            SqlDataReader readCheck;
            SqlDataReader readSinglePersonById;

            //check for ID
            cmdCheckPersonExists.CommandText = "SELECT count(id) FROM person_info WHERE id = " + personId + "";
            cmdCheckPersonExists.CommandType = CommandType.Text;
            cmdCheckPersonExists.Connection = checkCon;
            int checkPersonTrue = 0;

            try
            {
                checkCon.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                checkCon.Close();
                //this will be a 500 error on the Controller
                return null;
            }

            readCheck = cmdCheckPersonExists.ExecuteReader();

            while (readCheck.Read())
            {
                //grabs the count, eiter 0 or 1
                checkPersonTrue = readCheck.GetInt32(0);
            }

            readCheck.Close();

            //if found, return person and handle the 200 code on the Controller
            if (checkPersonTrue == 1)
            {
                cmdGetSingleById.CommandText = "SELECT * FROM person_info WHERE id = " + personId + "";
                cmdGetSingleById.CommandType = CommandType.Text;
                cmdGetSingleById.Connection = con;

                try
                {
                    con.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    con.Close();
                    //this will be a 500 error on the Controller
                    return null;
                }

                readSinglePersonById = cmdGetSingleById.ExecuteReader();

                while (readSinglePersonById.Read())
                {
                    tempPerson = new Person(
                    readSinglePersonById.GetInt32(0),
                    readSinglePersonById.GetString(1),
                    readSinglePersonById.GetString(2),
                    readSinglePersonById.GetString(3),
                    readSinglePersonById.GetInt32(4),
                    readSinglePersonById.GetString(5),
                    readSinglePersonById.GetString(6),
                    readSinglePersonById.GetInt32(7)
                    );
                }
                con.Close();
            }

            //if not found send back a code
            if (checkPersonTrue == 0)
            {
                return new Person("notFound", "notFound");
            }

            return tempPerson;
        }

        public int updatePerson(int updatedPersonId, Person updatedPerson)
        {
            SqlConnection checkCon = new SqlConnection(str);
            SqlConnection con = new SqlConnection(str);

            SqlCommand cmdCheckPersonExists = new SqlCommand();
            SqlCommand cmdUpdatePerson = new SqlCommand();

            SqlDataReader readCheck;

            //check for ID
            cmdCheckPersonExists.CommandText = "SELECT count(id) FROM person_info WHERE id = " + updatedPersonId + "";
            cmdCheckPersonExists.CommandType = CommandType.Text;
            cmdCheckPersonExists.Connection = checkCon;
            int checkPersonTrue = 0;

            try
            {
                checkCon.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                checkCon.Close();
                return 500;
            }

            readCheck = cmdCheckPersonExists.ExecuteReader();

            while (readCheck.Read())
            {
                checkPersonTrue = readCheck.GetInt32(0);
            }

            readCheck.Close();

            Console.WriteLine("checkPersonTrue: " + checkPersonTrue);

            //if ID exists, update
            if (checkPersonTrue == 1)
            {
                cmdUpdatePerson.CommandText = "UPDATE person_info SET "
                    + "firstName = '" + updatedPerson.firstName
                    + "', lastName = '" + updatedPerson.lastName
                    + "', streetName = '" + updatedPerson.streetName
                    + "', houseNumber = " + updatedPerson.houseNumber
                    + ", city = '" + updatedPerson.city
                    + "', state = '" + updatedPerson.state
                    + "', zipCode = " + updatedPerson.zipCode
                    + " WHERE id = " + updatedPersonId + "";
                cmdUpdatePerson.CommandType = CommandType.Text;
                cmdUpdatePerson.Connection = con;
                try
                {
                    con.Open();
                    cmdUpdatePerson.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    con.Close();
                    checkCon.Close();
                    return 500;
                }
                checkCon.Close();
                return 200;
            }

            //if ID does not exist, create the record
            else
            {
                checkCon.Close();
                insertPerson(updatedPerson);
                return 201;
            }

        }

        public bool insertPerson(Person newPerson)
        {
            SqlConnection con = new SqlConnection(str);
            SqlCommand cmdWritePerson = new SqlCommand();

            //cmdWriteTrack.CommandText = "INSERT demoitems VALUES ('Track Suit', 23.99)";
            cmdWritePerson.CommandText = "INSERT person_info VALUES ('"
                + newPerson.firstName + "','"
                + newPerson.lastName + "','"
                + newPerson.streetName + "',"
                + newPerson.houseNumber + ",'"
                + newPerson.city + "','"
                + newPerson.state + "',"
                + newPerson.zipCode + ")";
            cmdWritePerson.CommandType = CommandType.Text;
            cmdWritePerson.Connection = con;

            try
            {
                con.Open();
                cmdWritePerson.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                con.Close();
                return false;
            }

            con.Close();
            return true;
        }
    }
}