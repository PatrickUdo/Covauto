﻿namespace Covauto.Domain.Entities
{
    public class Auto
    {
        public int Id { get; set; }
        public string Kenteken { get; set; }
        public int Kilometerstand { get; set; }
        public string Naam { get; set; }
        public ICollection<LeenAutoReservering> Reserveringen { get; set; }
    }
}
