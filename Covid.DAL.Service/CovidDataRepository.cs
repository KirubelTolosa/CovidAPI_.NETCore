using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using Covid.DAL.Service.Models;
using Microsoft.Extensions.Configuration;
using Covid.DAL.Service.Utils;

namespace Covid.DAL.Service
{
    public class CovidDataRepository : ICovidDataRepository
    {
        private IConfiguration _configuration;
        public CovidDataRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void InsertGlobalLocations(List<CovidLocationDADto> dALRecords)
        {
            //List<CovidLocationDADto> dALRecords = Utils.Utilities.MapLocationsBLDTOtoDADTO(bALRecords);
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insert into LocationsGlobal(Country, State, Lat, Long) values (@Country, @State, @Lat, @Long)");
                SqlParameter Country = new SqlParameter("Country", System.Data.SqlDbType.NVarChar);
                SqlParameter State = new SqlParameter("State", System.Data.SqlDbType.NVarChar);
                SqlParameter Lat = new SqlParameter("Lat", System.Data.SqlDbType.Float);
                SqlParameter Long = new SqlParameter("Long", System.Data.SqlDbType.Float);
                command.Parameters.Add(Country);
                command.Parameters.Add(State);
                command.Parameters.Add(Lat);
                command.Parameters.Add(Long);
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("SampleTransaction");
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    foreach (var record in dALRecords)
                    {
                        Country.Value = record.Country;
                        State.Value = record.State;
                        Lat.Value = record.Lat;
                        Long.Value = record.Long;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }

            }
        }
        public void InsertUSLocations(List<CovidLocations_USDADto> dALRecords)
        {
            //List<CovidLocations_USDADto> dALRecords = Utils.Utilities.MapUSLocationsBLDTOtoDADTO(bALRecords);
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insert into Locations_us(County, State, Country, Combined_Key, Lat, Long) values (@County, @State, @Country, @Combined_Key, @Lat, @Long)");
                SqlParameter County = new SqlParameter("County", System.Data.SqlDbType.NVarChar);
                SqlParameter State = new SqlParameter("State", System.Data.SqlDbType.NVarChar);
                SqlParameter Country = new SqlParameter("Country", System.Data.SqlDbType.NVarChar);
                SqlParameter Combined_Key = new SqlParameter("Combined_Key", System.Data.SqlDbType.NVarChar);
                SqlParameter Lat = new SqlParameter("Lat", System.Data.SqlDbType.Float);
                SqlParameter Long = new SqlParameter("Long", System.Data.SqlDbType.Float);
                command.Parameters.Add(County);
                command.Parameters.Add(State);
                command.Parameters.Add(Country);
                command.Parameters.Add(Combined_Key);
                command.Parameters.Add(Lat);
                command.Parameters.Add(Long);
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("SampleTransaction");
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    foreach (var record in dALRecords)
                    {
                        County.Value = record.County;
                        State.Value = record.State;
                        Country.Value = record.Country;
                        Combined_Key.Value = record.Combined_Key;
                        Lat.Value = record.Lat;
                        Long.Value = record.Long;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
        }
        public void InsertNewGlobalLocations(string dbCompositeKey)
        {
            List<CovidLocationDADto> locationRecords = new List<CovidLocationDADto>();
            string[] keys = dbCompositeKey.Split('_');
            CovidLocationDADto locationRecord = new CovidLocationDADto
            {
                State = keys[0],
                Country = keys[1],
                Lat = Convert.ToDouble(keys[2]),
                Long = Convert.ToDouble(keys[3])
            };
            locationRecords.Add(locationRecord);
            InsertGlobalLocations(locationRecords);
        }
        public void InsertGlobalCases(List<CovidGlobalCaseCountDADto> dALRecords, Metrics metrics)
        {
            //List<CovidCaseCountDADto> dALRecords = Utils.Utilities.MapCaseCountsBLDTOtoDADTO(bALRecords);
            Dictionary<string, int> locationTableCompositeKeys = GetLocationTableCompositeKeys();
            string[] keys;
            double lat;
            double Long;
            foreach (var rec in dALRecords)
            {
                keys = rec.dbCompositeKey.Split('_');
                lat = Convert.ToDouble(keys[2]);
                Long = Convert.ToDouble(keys[3]);
                if (keys[2].Split('.')[1] == "0")
                {
                    keys[2] = keys[2].Split('.')[0];
                }
                else
                {
                    keys[2] = lat.ToString();
                }
                if (keys[3].Split('.')[1] == "0")
                {
                    keys[3] = keys[3].Split('.')[0];
                }
                else
                {
                    keys[3] = Long.ToString();
                }
                rec.dbCompositeKey = (keys[0] + '_' + keys[1] + '_' + keys[2] + '_' + keys[3]);
                if (!locationTableCompositeKeys.ContainsKey(rec.dbCompositeKey))
                {
                    InsertNewGlobalLocations(rec.dbCompositeKey);
                    locationTableCompositeKeys = GetLocationTableCompositeKeys();
                }

            }
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB")))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                if (metrics == Metrics.CONFIRMED_CASES)
                {
                    sqlCommand.CommandText = "insert into ConfirmedCases(Date, ConfirmedCasesCount, Id) values(@Date, @Count, @id)";
                }
                else if (metrics == Metrics.DEATHS)
                {
                    sqlCommand.CommandText = "insert into Deaths(Date, DeathCount, Id) values(@Date, @Count, @id)";
                }
                else if (metrics == Metrics.RECOVERIES)
                {
                    sqlCommand.CommandText = "insert into Recoveries(Date, RecoveriesCount, Id) values(@Date, @Count, @id)";
                }

                SqlParameter Date = new SqlParameter("Date", System.Data.SqlDbType.DateTime);
                SqlParameter Id = new SqlParameter("Id", System.Data.SqlDbType.Int);
                SqlParameter Count = new SqlParameter("Count", System.Data.SqlDbType.Int);

                sqlCommand.Parameters.Add(Date);
                sqlCommand.Parameters.Add(Id);
                sqlCommand.Parameters.Add(Count);
                SqlTransaction transaction = connection.BeginTransaction("InsertCases");
                sqlCommand.Connection = connection;
                sqlCommand.Transaction = transaction;
                try
                {
                    foreach (var record in dALRecords)
                    {
                        Date.Value = record.Date;
                        Count.Value = record.Count;
                        Id.Value = locationTableCompositeKeys[record.dbCompositeKey];
                        sqlCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
        }
        public void InsertUSCases(List<CovidUSCaseCountDADto> dALRecords, Metrics metrics)
        {
            //List<CovidUSCaseCountDADto> dALRecords = Utils.Utilities.MapUSCaseCountsBLDTOtoDADTO(bALRecords);
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB")))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                if (metrics == Metrics.CONFIRMED_CASES)
                {
                    sqlCommand.CommandText = "insert into ConfirmedCases_US(Combined_Key, Date, ConfirmedCasesCount) values(@Combined_Key, @Date, @Count)";
                }
                else if (metrics == Metrics.DEATHS)
                {
                    sqlCommand.CommandText = "insert into Deaths_US(Combined_Key, Date, DeathCount) values(@Combined_Key, @Date, @Count)";
                }
                SqlParameter Combined_Key = new SqlParameter("Combined_Key", System.Data.SqlDbType.NVarChar);
                SqlParameter Date = new SqlParameter("Date", System.Data.SqlDbType.DateTime);
                SqlParameter Count = new SqlParameter("Count", System.Data.SqlDbType.Int);

                sqlCommand.Parameters.Add(Combined_Key);
                sqlCommand.Parameters.Add(Date);
                sqlCommand.Parameters.Add(Count);
                SqlTransaction transaction = connection.BeginTransaction("InsertUSCases");
                sqlCommand.Connection = connection;
                sqlCommand.Transaction = transaction;
                try
                {
                    foreach (var record in dALRecords)
                    {
                        Combined_Key.Value = record.Combined_Key;
                        Date.Value = record.Date;
                        Count.Value = record.Count;
                        sqlCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
        }
        public Dictionary<string, int> GetLocationTableCompositeKeys()
        {
            Dictionary<string, int> locationTableCompositeKeys = new Dictionary<string, int>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB"));
            SqlCommand sqlCommand = new SqlCommand("SELECT Id, [State], Country, Lat, Long  FROM LocationsGlobal", connection);
            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            string state = "Null";
            string country = "Null";
            double Lat = 0;
            double Long = 0;
            try
            {
                while (reader.Read())
                {
                    if ((string)reader["State"] != "")
                    {
                        state = (string)reader["State"];
                    }
                    if ((string)reader["Country"] != "")
                    {
                        country = (string)reader["Country"];
                    }
                    if ((reader["Lat"]) != null)
                    {
                        Lat = Convert.ToDouble(reader["Lat"]);
                    }
                    if (reader["Long"] != null)
                    {
                        Long = Convert.ToDouble(reader["Long"]);
                    }
                    locationTableCompositeKeys.Add((state + '_' + country + '_' + Lat + '_' + Long), (int)reader["Id"]);
                    state = "Null";
                    country = "Null";
                    Lat = 0;
                    Long = 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return locationTableCompositeKeys;
        }
        public DateTime GetLastUpdateDate(Metrics metrics, Locations location)
        {
            SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB"));
            SqlCommand sqlCommandConfirmedCases = new SqlCommand("SELECT MAX ([Date]) FROM ConfirmedCases Where ID = 1", sqlConnection);
            SqlCommand sqlCommandDeaths = new SqlCommand("SELECT MAX ([Date]) FROM Deaths Where ID = 1", sqlConnection);
            SqlCommand sqlCommandRecoveries = new SqlCommand("SELECT MAX ([Date]) FROM Recoveries Where ID = 1", sqlConnection);

            SqlCommand sqlCommandConfirmedCases_US = new SqlCommand("SELECT MAX ([Date]) FROM ConfirmedCases_US Where Combined_Key = 'American Samoa, US'", sqlConnection);
            SqlCommand sqlCommandDeaths_US = new SqlCommand("SELECT MAX ([Date]) FROM Deaths_US Where Combined_Key = 'American Samoa, US'", sqlConnection);

            DateTime LastUpdateDate = new DateTime();
            try
            {
                sqlConnection.Open();
                if (metrics == Metrics.CONFIRMED_CASES && location != Locations.US)
                {
                    LastUpdateDate = (DateTime)sqlCommandConfirmedCases.ExecuteScalar();
                }
                else if (metrics == Metrics.DEATHS && location != Locations.US)
                {
                    LastUpdateDate = (DateTime)sqlCommandDeaths.ExecuteScalar();
                }
                else if (metrics == Metrics.RECOVERIES && location != Locations.US)
                {
                    LastUpdateDate = (DateTime)sqlCommandRecoveries.ExecuteScalar();
                }
                else if (metrics == Metrics.CONFIRMED_CASES && location == Locations.US)
                {
                    LastUpdateDate = (DateTime)sqlCommandConfirmedCases_US.ExecuteScalar();
                }
                else if (metrics == Metrics.DEATHS && location == Locations.US)
                {
                    LastUpdateDate = (DateTime)sqlCommandDeaths_US.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return LastUpdateDate;
        }
        public List<NationalCasesDADto> GetCountOfCasesForAllNations(Metrics metrics)
        {
            List<NationalCasesDADto> covidCasesDALRecords = new List<NationalCasesDADto>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB"));
            SqlCommand sqlCommand = new SqlCommand(@"select Country, sum(Count) as Count
                                                    from 
                                                    (
	                                                    select Country, [state], MAX(c.ConfirmedCasesCount) as Count  
	                                                    from ConfirmedCases c
	                                                    inner join LocationsGlobal l on c.ID = l.ID	
	                                                    group by [state], l.Country
                                                    ) numbers
                                                    group by Country", connection);
            if (metrics == Metrics.DEATHS)
            {
                sqlCommand = new SqlCommand(@"select Country, sum(Count) as Count
                                                from 
                                                (
	                                                select Country, [state], MAX(d.DeathCount) as Count  
	                                                from DEATHS d
	                                                inner join LocationsGlobal l on d.ID = l.ID	
	                                                group by [state], l.Country
                                                ) numbers
                                                group by Country", connection);
            }
            else if (metrics == Metrics.RECOVERIES)
            {
                sqlCommand = new SqlCommand(@"select Country, sum(Count) as Count 
                                                from 
                                                (
	                                                select Country, [state], MAX(r.RecoveriesCount) as Count  
	                                                from RECOVERIES r
	                                                inner join LocationsGlobal l on r.ID = l.ID	
	                                                group by [state], l.Country
                                                ) numbers
                                                group by Country", connection);
            }
            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    NationalCasesDADto CaseCountDto = new NationalCasesDADto
                    {
                        Country = (string)reader["Country"],
                        Count = (Int32)reader["Count"],
                    };
                    covidCasesDALRecords.Add(CaseCountDto);
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return covidCasesDALRecords;
        }
        public List<NationalCasesDADto> GetCountOfCasesByCountry(Metrics metrics, string Country, DateTime? Date)
        {
            List<NationalCasesDADto> covidCasesDALRecords = new List<NationalCasesDADto>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB"));

            SqlCommand sqlCommand = new SqlCommand(@"select Country, sum(Count) as Count
                                                    from 
                                                    (
	                                                    select Country, [state], MAX(c.ConfirmedCasesCount) as Count  
	                                                    from ConfirmedCases c
	                                                    inner join LocationsGlobal l on c.ID = l.ID	
                                                        WHERE l.Country = @ctry 
	                                                    group by [state], l.Country
                                                    ) numbers
                                                    group by Country", connection);
            if (Date != DateTime.MinValue)
            {
                sqlCommand = new SqlCommand(@"select Country, sum(Count) as Count
                                                from 
                                                (
	                                                   select Country, [state], MAX(c.ConfirmedCasesCount) as Count  
	                                                   from ConfirmedCases c
	                                                   inner join LocationsGlobal l on c.ID = l.ID	
                                                       WHERE Country = @ctry  and c.[Date] = @dte  
	                                                   group by [state], l.Country
                                                ) numbers
                                                group by Country", connection);
            }
            if (metrics == Metrics.DEATHS)
            {
                if (Date == DateTime.MinValue)
                {
                    sqlCommand = new SqlCommand(@"select Country, sum(Count) as Count
                                                    from 
                                                    (
	                                                    select Country, [state], MAX(d.DeathCount) as Count  
	                                                    from DEATHS d
	                                                    inner join LocationsGlobal l on d.ID = l.ID	
                                                        WHERE l.Country = @ctry 
	                                                    group by [state], l.Country
                                                    ) numbers
                                                    group by Country", connection);
                }
                else
                {
                    sqlCommand = new SqlCommand(@"select Country, sum(Count) as Count
                                                    from 
                                                    (
	                                                    select Country, [state], MAX(d.DeathCount) as Count  
	                                                    from DEATHS d
	                                                    inner join LocationsGlobal l on d.ID = l.ID	
                                                        WHERE Country = @ctry  and d.[Date] = @dte 
	                                                    group by [state], l.Country
                                                    ) numbers
                                                    group by Country", connection);
                }
            }
            else if (metrics == Metrics.RECOVERIES)
            {
                if (Date == DateTime.MinValue)
                {
                    sqlCommand = new SqlCommand(@"select Country, sum(Count) as Count 
                                                from 
                                                (
	                                                select Country, [state], MAX(r.RecoveriesCount) as Count  
	                                                from RECOVERIES r
	                                                inner join LocationsGlobal l on r.ID = l.ID	
                                                    WHERE Country = @ctry  
	                                                group by [state], l.Country
                                                ) numbers
                                                group by Country", connection);
                }
                else
                {
                    sqlCommand = new SqlCommand(@"select Country, sum(Count) as Count 
                                                from 
                                                (
	                                                select Country, [state], MAX(r.RecoveriesCount) as Count  
	                                                from RECOVERIES r
	                                                inner join LocationsGlobal l on r.ID = l.ID	
                                                    WHERE Country = @ctry and d.[Date] = @dte 
	                                                group by [state], l.Country
                                                ) numbers
                                                group by Country", connection);
                }

            }
            SqlParameter ctry = new SqlParameter("ctry", System.Data.SqlDbType.NVarChar);
            ctry.Value = Country;
            sqlCommand.Parameters.Add(ctry);
            if (Date != DateTime.MinValue)
            {
                SqlParameter dte = new SqlParameter("dte", System.Data.SqlDbType.NVarChar);
                dte.Value = Date;
                sqlCommand.Parameters.Add(dte);
            }

            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    NationalCasesDADto CaseCountDto = new NationalCasesDADto
                    {
                        Country = (string)reader["Country"],
                        Count = (Int32)reader["Count"],
                    };
                    covidCasesDALRecords.Add(CaseCountDto);
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return covidCasesDALRecords;
        }
        public List<USCountyCaseCountDADto> GetCountOfUSCasesByCounty(Metrics metrics, Locations location, string State, string County, DateTime? Date)
        {
            List<USCountyCaseCountDADto> covidUSCountyCasesDALRecords = new List<USCountyCaseCountDADto>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB"));

            SqlParameter St = new SqlParameter("St", System.Data.SqlDbType.NVarChar);
            St.Value = State;
            SqlParameter Cty = new SqlParameter("Cty", System.Data.SqlDbType.NVarChar);
            Cty.Value = County;

            SqlCommand sqlCommand = new SqlCommand(@"select [State], [County], MAX(CUS.ConfirmedCasesCount) AS 'Count' from Locations_US l INNER JOIN ConfirmedCases_US CUS ON CUS.Combined_Key = l.Combined_Key where l.[State] = @St and l.County = @Cty group by l.[State], l.County", connection);
            if (Date == DateTime.MinValue)
            {
                Date = GetLastUpdateDate(metrics, location).Date;
            }
            if (Date != DateTime.MinValue)
            {
                sqlCommand = new SqlCommand(@"select [State], [County], MAX(CUS.ConfirmedCasesCount) AS Count
														from Locations_US l INNER JOIN ConfirmedCases_US CUS ON CUS.Combined_Key = l.Combined_Key
														where l.[State] = @St and l.County = @Cty and CUS.[Date] = @dte
														group by l.[State], l.County", connection);
            }
            if (metrics == Metrics.DEATHS)
            {
                if (Date == DateTime.MinValue)
                {
                    sqlCommand = new SqlCommand(@"select [State], [County], Max(d.DeathCount) AS Count
                                                        from Locations_US l INNER JOIN  DEATHS_US d ON  d.Combined_Key = l.Combined_Key 
                                                        WHERE l.[State] = @St and l.County = @Cty  
                                                        group by l.[State], l.County", connection);
                }
                else
                {
                    sqlCommand = new SqlCommand(@"select [State], [County], Max(d.DeathCount) AS Count 
                                                        from Locations_US l INNER JOIN  DEATHS_US d ON  d.Combined_Key = l.Combined_Key 
                                                        WHERE l.[State] = @St and l.County = @Cty and d.Date = @dte 
                                                        group by l.[State], l.County", connection);
                }
            }

            sqlCommand.Parameters.Add(St);
            sqlCommand.Parameters.Add(Cty);
            if (Date != DateTime.MinValue)
            {
                SqlParameter dte = new SqlParameter("dte", System.Data.SqlDbType.DateTime);
                dte.Value = (DateTime)Date;
                sqlCommand.Parameters.Add(dte);
            }

            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    USCountyCaseCountDADto CaseCountDto = new USCountyCaseCountDADto
                    {
                        State = (string)reader["State"],
                        County = (string)reader["County"],
                        Date = (DateTime)Date,
                        Count = (Int32)reader["Count"],
                    };
                    covidUSCountyCasesDALRecords.Add(CaseCountDto);
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return covidUSCountyCasesDALRecords;
        }
        public List<USStateCaseCountDADto> GetCountOfUSCasesByState(Metrics metrics, Locations locations, string State, DateTime? Date)
        {
            List<USStateCaseCountDADto> covidUSStateCasesDALRecords = new List<USStateCaseCountDADto>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB"));

            SqlParameter St = new SqlParameter("St", System.Data.SqlDbType.NVarChar);
            St.Value = State;
            SqlCommand sqlCommand = new SqlCommand(@"select [State], sum(Count) as Count from (select [state], county, MAX(c.ConfirmedCasesCount) as Count from ConfirmedCases_US c where [state] = @St group by [state], l.County) numbers group by [state]", connection);
            if (Date == DateTime.MinValue)
            {
                Date = GetLastUpdateDate(metrics, locations).Date;
            }
            if (Date != DateTime.MinValue)
            {
                sqlCommand = new SqlCommand(@"select [State], sum(Count) as Count
                                                        from 
                                                        (
	                                                        select [state], county, MAX(c.ConfirmedCasesCount) as Count  
	                                                        from ConfirmedCases_US c
	                                                        inner join Locations_US l on c.Combined_Key = l.Combined_Key	
	                                                        where [state] = @St and [Date] = @dte
	                                                        group by [state], l.County	
                                                        ) numbers
                                                        group by [state]", connection);
            }
            if (metrics == Metrics.DEATHS)
            {
                if (Date == DateTime.MinValue)
                {
                    sqlCommand = new SqlCommand(@"select [State], sum(Count) as Count
                                                        from 
                                                        (
	                                                        select [state], county, MAX(d.DeathCount) as Count  
	                                                        from DEATHS_US d
	                                                        inner join Locations_US l on d.Combined_Key = l.Combined_Key
	                                                        where [state] =  @St
	                                                        group by [state], l.County	
                                                        ) numbers
                                                        group by [state]", connection);
                }
                else
                {
                    sqlCommand = new SqlCommand(@"select [State], sum(Count) as Count
                                                        from 
                                                        (
	                                                        select [state], county, MAX(d.DeathCount) as Count  
	                                                        from DEATHS_US d
	                                                        inner join Locations_US l on d.Combined_Key = l.Combined_Key
	                                                        where [state] =  @St and [Date] = @dte
	                                                        group by [state], l.County	
                                                        ) numbers
                                                        group by [state]", connection);
                }
            }

            sqlCommand.Parameters.Add(St);
            if (Date != DateTime.MinValue)
            {
                SqlParameter dte = new SqlParameter("dte", System.Data.SqlDbType.DateTime);
                dte.Value = (DateTime)Date;
                sqlCommand.Parameters.Add(dte);
            }
            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    USStateCaseCountDADto CaseCountDto = new USStateCaseCountDADto();
                    CaseCountDto.State = (string)reader["State"];
                    CaseCountDto.Count = (Int32)reader["Count"];
                    if (Date != DateTime.MinValue)
                    {
                        CaseCountDto.Date = (DateTime)Date;
                    }

                    covidUSStateCasesDALRecords.Add(CaseCountDto);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return covidUSStateCasesDALRecords;
        }
        public List<GlobalTotalCountsDADto> GetGlobalTotalCounts(Metrics metrics)
        {
            List<GlobalTotalCountsDADto> globalTotalCases = new List<GlobalTotalCountsDADto>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB"));
            SqlCommand sqlCommand = new SqlCommand("select sum(DISTINCT ConfirmedCases.ConfirmedCasesCount) AS 'Count' FROM LocationsGlobal INNER JOIN  ConfirmedCases ON  ConfirmedCases.Id = LocationsGlobal.Id WHERE ConfirmedCases.[Date] = (select MAX([Date]) FROM ConfirmedCases)", connection);
            connection.Open();
            if (metrics == Metrics.DEATHS)
            {
                sqlCommand = new SqlCommand("select sum(DISTINCT Deaths.DeathCount) AS 'Count' FROM LocationsGlobal INNER JOIN  Deaths ON  LocationsGlobal.Id = Deaths.Id WHERE Deaths.[Date] = (select MAX([Date]) FROM ConfirmedCases)", connection);
            }
            else if (metrics == Metrics.RECOVERIES)
            {
                sqlCommand = new SqlCommand("select sum(DISTINCT Recoveries.RecoveriesCount) AS 'Count' FROM LocationsGlobal INNER JOIN  Recoveries ON  Recoveries.Id = LocationsGlobal.Id WHERE Recoveries.[Date] = (select MAX([Date]) FROM ConfirmedCases) ", connection);
            }

            SqlDataReader reader = sqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    GlobalTotalCountsDADto CaseCountDto = new GlobalTotalCountsDADto
                    {
                        Count = (int)reader["Count"]
                    };
                    globalTotalCases.Add(CaseCountDto);
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return globalTotalCases;
        }
        public List<DailyCaseCountsDADto> GetDailyCaseCountsByCountry(Metrics metrics, string Country)
        {
            List<DailyCaseCountsDADto> DailyCaseCounts = new List<DailyCaseCountsDADto>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB"));
            SqlCommand sqlCommand = new SqlCommand("SELECT ConfirmedCases.[Date], ConfirmedCases.ConfirmedCasesCount As Count FROM ConfirmedCases INNER JOIN LocationsGlobal ON LocationsGlobal.Id = ConfirmedCases.Id WHERE LocationsGlobal.Country = @ctry", connection);
            connection.Open();
            if (metrics == Metrics.DEATHS)
            {
                sqlCommand = new SqlCommand("SELECT Deaths.[Date], Deaths.DeathCount As Count FROM Deaths INNER JOIN LocationsGlobal ON LocationsGlobal.Id = Deaths.Id WHERE LocationsGlobal.Country = @ctry", connection);
            }
            else if (metrics == Metrics.RECOVERIES)
            {
                sqlCommand = new SqlCommand("SELECT Recoveries.[Date], Recoveries.RecoveriesCount As Count FROM Recoveries INNER JOIN LocationsGlobal ON LocationsGlobal.Id = Recoveries.Id WHERE LocationsGlobal.Country = @ctry", connection);
            }
            SqlParameter ctry = new SqlParameter("ctry", System.Data.SqlDbType.NVarChar);
            ctry.Value = Country;
            sqlCommand.Parameters.Add(ctry);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    DailyCaseCountsDADto DailyCaseCountsDto = new DailyCaseCountsDADto
                    {
                        Date = (DateTime)reader["Date"],
                        Count = (int)reader["Count"]
                    };
                    DailyCaseCounts.Add(DailyCaseCountsDto);
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return DailyCaseCounts;
        }
        public List<LocationNameDADto> GetListOfLocations(Locations locations, string region)
        {
            List<LocationNameDADto> locationNames = new List<LocationNameDADto>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MSSQL_MainDB")))
            {
                connection.Open(); 
                SqlCommand sqlCommand = new SqlCommand();

                if (locations == Locations.Global && region == "nation")
                {
                    sqlCommand.CommandText = "select distinct Country as Country from LocationsGlobal";
                }
                else if (locations == Locations.US && region == "state")
                {
                    sqlCommand.CommandText = "select distinct state as [State] from Locations_US order by [State]";
                }
                else if (locations == Locations.US && region == "county")
                {
                    sqlCommand.CommandText = "Select distinct county as County from Locations_US order by County";
                }
                sqlCommand.Connection = connection;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        LocationNameDADto location = new LocationNameDADto();
                        location.location = (locations == Locations.Global) ? (string)reader["Country"] :
                                            (locations == Locations.US && region == "state") ? (string)reader["State"] :
                                            (locations == Locations.US && region == "county") ? (string)reader["County"] : "Use lower case letters for your region";
                        locationNames.Add(location);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }            
            return locationNames;
        }
   
    }
}




