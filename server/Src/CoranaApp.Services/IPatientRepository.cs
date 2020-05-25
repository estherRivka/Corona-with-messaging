﻿using CoronaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaApp.Services
{
    public interface IPatientRepository
    {
        Patient GetById(int id);

        Patient Update(Patient patient);
        Patient Save(Patient newPatient);
        //void Delete(int id);
    }
}
