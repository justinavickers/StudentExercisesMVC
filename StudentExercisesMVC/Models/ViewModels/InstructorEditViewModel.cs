﻿using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class InstructorEditViewModel
    {
            // A single instructor
            public Instructor Instructor { get; set; } = new Instructor();

            // All cohorts
            public List<SelectListItem> Cohorts;

            public SqlConnection Connection;


            public InstructorEditViewModel() { }

            public InstructorEditViewModel(SqlConnection connection)
            {
                Connection = connection;
                GetAllCohorts();
            }
        
            public void GetAllCohorts()
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT c.Id, c.Designation from Cohort c";

                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Cohort> cohorts = new List<Cohort>();

                        while (reader.Read())
                        {
                            Cohort cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Designation = reader.GetString(reader.GetOrdinal("Designation"))
                            };

                            cohorts.Add(cohort);
                        }

                        Cohorts = cohorts.Select(li => new SelectListItem
                        {
                            Text = li.Designation,
                            Value = li.Id.ToString()
                        }).ToList();

                        Cohorts.Insert(0, new SelectListItem
                        {
                            Text = "Choose cohort...",
                            Value = "0"
                        });
                    }
                }
            }
        
    }
}
