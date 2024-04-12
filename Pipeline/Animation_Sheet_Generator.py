from PIL import Image
import glob
import os

# FOLDERS AND FILES PARAMETERS
SAMPLES_DIR_FACE: str = r"Assets\Sprites\Character\Rat\Rat_Face_statique\Face_STATIQUE"
SAMPLES_DIR_PROFILE: str = r"Assets\Sprites\Character\Rat\Rat_Profil_statique\Profil_statique"
SAMPLES_DIR_BACK: str = r"Assets\Sprites\Character\Rat\Rat_Dos_statique\Dos_statique"
DIRECTORIES_TO_PARSE: list[str] = [SAMPLES_DIR_FACE, SAMPLES_DIR_PROFILE, SAMPLES_DIR_BACK]
OUTPUT_DIR: str = r"Assets\Sprites\Character\Rat"
OUTPUT_FILE: str = r"Rat_AnimationSheet.png"

# IMAGE PARAMETERS
CROP: bool = True
CROP_COORDS: tuple[int, int, int, int] = (370, 380, 1730, 1020)  # (left, upper, right, lower)
RESIZE: bool = True
RESIZE_WIDTH: int = 600

images: list[str] = []
output_image: Image.Image
mode: str


def concat_images(image1: Image.Image, image2: Image.Image) -> Image.Image:
    concat_image: Image.Image = Image.new(mode, (image1.width + image2.width, image1.height))
    concat_image.paste(image1, (0, 0))
    concat_image.paste(image2, (image1.width, 0))
    return concat_image


def open_image(file: str) -> Image.Image:
    image = Image.open(file)

    if CROP:
        if image.width < CROP_COORDS[2] or image.height < CROP_COORDS[3]:
            raise ValueError(f"${file} : image is too small to be cropped")

        image = image.crop(CROP_COORDS)

    if RESIZE:
        image = image.resize((RESIZE_WIDTH, int(image.height * (RESIZE_WIDTH / image.width))))

    return image


if __name__ == "__main__":

    for directory in DIRECTORIES_TO_PARSE:
        files = glob.glob(directory + "/*.png")
        images.extend(files)

    for i in range(len(images) - 1):
        print(f"Concatenating ${images[i + 1]}")

        if i == 0:
            output_image = open_image(images[i])
            mode = output_image.mode

        image2 = open_image(images[i + 1])
        output_image = concat_images(output_image, image2)

    print(f"Saving to {os.path.join(OUTPUT_DIR, OUTPUT_FILE)}")
    output_image.save(os.path.join(OUTPUT_DIR, OUTPUT_FILE))
    print("Done")
