# Generatore di audio wav by MoonDragon-MD (https://github.com/MoonDragon-MD/ArtOfRallyCoPilot)
# pip install edge-tts
# python3 genera-audio-wav.py
import edge_tts
import asyncio
import os

# Dizionario delle frasi con nomi file corrispondenti
phrases = {
    # Italiano
    "R": "destra...",
    "1R": "destra uno",
    "2R": "destra due",
    "3R": "destra tre",
    "4R": "destra quattro",
    "5R": "destra cinque",
    "6R": "destra sei",
    "L": "sinistra...",
    "1L": "sinistra uno",
    "2L": "sinistra due",
    "3L": "sinistra tre",
    "4L": "sinistra quattro",
    "5L": "sinistra cinque",
    "6L": "sinistra sei",
    "HpR": "stretta destra",
    "HpL": "stretta sinistra",
    "AcR": "gomito destra",
    "AcL": "gomito sinistra",
    "SqR": "novanta destra",
    "SqL": "novanta sinistra",
    "extra": "attenzione dosso",
    "extra5": "attenzione cunetta",
    "extra1": "attenzione ponte",
    "extra2": "attenzione si restringe",
    "extra3": "stai largo",
    "extra4": "stai stretto",

    # English
    "R_en": "right...",
    "1R_en": "right one",
    "2R_en": "right two",
    "3R_en": "right three",
    "4R_en": "right four",
    "5R_en": "right five",
    "6R_en": "right six",
    "L_en": "left...",
    "1L_en": "left one",
    "2L_en": "left two",
    "3L_en": "left three",
    "4L_en": "left four",
    "5L_en": "left five",
    "6L_en": "left six",
    "HpR_en": "tight right",
    "HpL_en": "tight left",
    "AcR_en": "elbow right",
    "AcL_en": "elbow left",
    "SqR_en": "ninety right",
    "SqL_en": "ninety left",
    "extra_en": "caution bump",
    "extra5_en": "caution dip",
    "extra1_en": "caution bridge",
    "extra2_en": "caution narrowing",
    "extra3_en": "stay wide",
    "extra4_en": "stay tight",

    # Deutsch
    "R_de": "rechts...",
    "1R_de": "rechts eins",
    "2R_de": "rechts zwei",
    "3R_de": "rechts drei",
    "4R_de": "rechts vier",
    "5R_de": "rechts fünf",
    "6R_de": "rechts sechs",
    "L_de": "links...",
    "1L_de": "links eins",
    "2L_de": "links zwei",
    "3L_de": "links drei",
    "4L_de": "links vier",
    "5L_de": "links fünf",
    "6L_de": "links sechs",
    "HpR_de": "enge rechts",
    "HpL_de": "enge links",
    "AcR_de": "ellenbogen rechts",
    "AcL_de": "ellenbogen links",
    "SqR_de": "neunzig rechts",
    "SqL_de": "neunzig links",
    "extra_de": "achtung buckel",
    "extra5_de": "achtung senke",
    "extra1_de": "achtung brücke",
    "extra2_de": "achtung enge",
    "extra3_de": "bleib breit",
    "extra4_de": "bleib eng",

    # Français
    "R_fr": "droite...",
    "1R_fr": "droite un",
    "2R_fr": "droite deux",
    "3R_fr": "droite trois",
    "4R_fr": "droite quatre",
    "5R_fr": "droite cinq",
    "6R_fr": "droite six",
    "L_fr": "gauche...",
    "1L_fr": "gauche un",
    "2L_fr": "gauche deux",
    "3L_fr": "gauche trois",
    "4L_fr": "gauche quatre",
    "5L_fr": "gauche cinq",
    "6L_fr": "gauche six",
    "HpR_fr": "serré droite",
    "HpL_fr": "serré gauche",
    "AcR_fr": "coude droite",
    "AcL_fr": "coude gauche",
    "SqR_fr": "quatre-vingt-dix droite",
    "SqL_fr": "quatre-vingt-dix gauche",
    "extra_fr": "attention bosse",
    "extra5_fr": "attention creux",
    "extra1_fr": "attention pont",
    "extra2_fr": "attention rétrécissement",
    "extra3_fr": "reste large",
    "extra4_fr": "reste serré"
}

async def generate_and_convert_audio(voice, text, output_folder, filename):
    # Genera il file audio
    output_file = os.path.join(output_folder, f"{filename}.wav")
    communicate = edge_tts.Communicate(text, voice)
    await communicate.save(output_file)
    print(f"Generato: {output_file}")

    # Ricodifica con ffmpeg
    converted_file = os.path.join(output_folder, f"converted_{filename}.wav")
    os.system(f'ffmpeg -i "{output_file}" -acodec pcm_s16le -ac 2 -ar 44100 -b:a 1411k -y "{converted_file}"')
    os.remove(output_file)  # Elimina il file originale
    os.rename(converted_file, output_file)  # Rinomina il file convertito
    print(f"Convertito: {output_file}")

def main():
    # Crea cartelle per lingue e generi
    for folder in ["ita-f", "ita", "eng-f", "default", "deu-f", "deu", "fra-f", "fra"]:
        os.makedirs(os.path.join("audio_output", folder), exist_ok=True)

    # Genera file in italiano (femminile e maschile)
    for filename, text in phrases.items():
        if filename.endswith(("_en", "_de", "_fr")):
            continue
        # Italiano femminile
        loop = asyncio.get_event_loop()
        loop.run_until_complete(generate_and_convert_audio("it-IT-ElsaNeural", text, "audio_output/ita-f", filename))
        # Italiano maschile
        loop.run_until_complete(generate_and_convert_audio("it-IT-DiegoNeural", text, "audio_output/ita", filename))

    # Genera file in inglese (femminile e maschile)
    for filename, text in phrases.items():
        if not filename.endswith("_en"):
            continue
        base_name = filename.replace("_en", "")
        # Inglese femminile
        loop = asyncio.get_event_loop()
        loop.run_until_complete(generate_and_convert_audio("en-US-JennyNeural", text, "audio_output/eng-f", base_name))
        # Inglese maschile
        loop.run_until_complete(generate_and_convert_audio("en-US-GuyNeural", text, "audio_output/default", base_name))

    # Genera file in tedesco (femminile e maschile)
    for filename, text in phrases.items():
        if not filename.endswith("_de"):
            continue
        base_name = filename.replace("_de", "")
        # Tedesco femminile
        loop = asyncio.get_event_loop()
        loop.run_until_complete(generate_and_convert_audio("de-DE-KatjaNeural", text, "audio_output/deu-f", base_name))
        # Tedesco maschile
        loop.run_until_complete(generate_and_convert_audio("de-DE-ConradNeural", text, "audio_output/deu", base_name))

    # Genera file in francese (femminile e maschile)
    for filename, text in phrases.items():
        if not filename.endswith("_fr"):
            continue
        base_name = filename.replace("_fr", "")
        # Francese femminile
        loop = asyncio.get_event_loop()
        loop.run_until_complete(generate_and_convert_audio("fr-FR-DeniseNeural", text, "audio_output/fra-f", base_name))
        # Francese maschile
        loop.run_until_complete(generate_and_convert_audio("fr-FR-HenriNeural", text, "audio_output/fra", base_name))

if __name__ == "__main__":
    # Esegui l'asyncio in modo sincrono
    loop = asyncio.get_event_loop()
    loop.run_until_complete(main())