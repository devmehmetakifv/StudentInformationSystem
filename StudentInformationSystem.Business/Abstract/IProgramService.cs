﻿using StudentInformationSystem.Business.Generic.Abstract;
using StudentInformationSystem.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Business.Abstract
{
	public interface IProgramService : IGenericService<Program>
	{
		public int GetProgramIdByName(string programName);
	}
}
