using System.Collections.Generic;
using UnityEngine;

public class Enfermedad : MonoBehaviour
{

    public enum TipoSintoma { A_Calor, B_Flujos, C_Cuerpo, D_Dolor, E_Conducta }
    public enum Enfermedades { CATARRO, SARPULLIDO, RAMAS_BRAZOS, OTITIS, OJOS_CARACOL, LEPRA, BICHOS_OJOS }

    Dictionary<Enfermedades, TipoSintoma[]> enfermedades = new Dictionary<Enfermedades, TipoSintoma[]>();

    public Enfermedades[] leves;

    // Start is called once beforez the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enfermedades[Enfermedades.CATARRO] = new TipoSintoma[] { TipoSintoma.A_Calor, TipoSintoma.B_Flujos, TipoSintoma.D_Dolor };
        enfermedades[Enfermedades.SARPULLIDO] = new TipoSintoma[] { TipoSintoma.A_Calor, TipoSintoma.B_Flujos, TipoSintoma.C_Cuerpo };
        enfermedades[Enfermedades.RAMAS_BRAZOS] = new TipoSintoma[] {
    TipoSintoma.C_Cuerpo,
    TipoSintoma.D_Dolor,
    TipoSintoma.E_Conducta
};

        enfermedades[Enfermedades.OTITIS] = new TipoSintoma[] {
    TipoSintoma.B_Flujos,
    TipoSintoma.D_Dolor,
    TipoSintoma.E_Conducta
};

        enfermedades[Enfermedades.OJOS_CARACOL] = new TipoSintoma[] {
    TipoSintoma.A_Calor,
    TipoSintoma.E_Conducta,
    TipoSintoma.B_Flujos
};

        enfermedades[Enfermedades.LEPRA] = new TipoSintoma[] {
    TipoSintoma.C_Cuerpo,
    TipoSintoma.D_Dolor,
    TipoSintoma.A_Calor
};

        enfermedades[Enfermedades.BICHOS_OJOS] = new TipoSintoma[] {
    TipoSintoma.B_Flujos,
    TipoSintoma.C_Cuerpo,
    TipoSintoma.E_Conducta
};

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TipoSintoma[] getSintomas(Enfermedades enfermedad)
    {
        return enfermedades[enfermedad];
    }
}
