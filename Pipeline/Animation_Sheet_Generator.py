from PIL import Image
import glob
import os


# ======================== FOLDERS AND FILES PARAMETERS =======================


DIRECTORIES_TO_PARSE: list[str] = [
        r"Assets\Sprites\Character\Rat\Moving_Rat\Back",
        r"Assets\Sprites\Character\Rat\Moving_Rat\Front",
        r"Assets\Sprites\Character\Rat\Moving_Rat\Profile"
    ]
OUTPUT_DIR: str = r"Assets\Sprites\Character\Rat"
OUTPUT_FILES_NAME: str = "Moving_Rat_AnimationSheet"
OUTPUT_EXTENSION: str = "png"


# ============================== IMAGE PARAMETERS =============================


CROP: bool = True

# (left, upper, right, lower)
# CROP_COORDS: tuple[int, int, int, int] = (370, 380, 1730, 1020)  # Idle rat
# CROP_COORDS: tuple[int, int, int, int] = (500, 0, 1530, 1080)  # Garde
CROP_COORDS: tuple[int, int, int, int] = (40, 240, 1830, 980)  # Moving rat
# CROP_COORDS: tuple[int, int, int, int] = (990, 660, 1350, 1040)  # Apple

RESIZE: bool = False
RESIZE_WIDTH: int = 600

TILES_PER_ROW: int = 7


# ============================ FUNCTIONS DEFINITION ===========================


def open_image(file: str) -> Image.Image:
    image = Image.open(file)

    if CROP:
        if image.width < CROP_COORDS[2] or image.height < CROP_COORDS[3]:
            raise ValueError(f"${file} : image is too small to be cropped")

        image = image.crop(CROP_COORDS)

    if RESIZE:
        image = image.resize(
            (
                RESIZE_WIDTH,
                int(image.height * (RESIZE_WIDTH / image.width))
            )
        )

    return image


# ============================= CLASSES DEFINITION ============================


class AnimationSheetGenerator:
    _nb_rows: int
    _concat_image_width: int
    _concat_image_height: int
    _mode: str
    _concat_image: Image.Image

    def __init__(self, nb_images: int, images_width: int, images_height: int, mode: str):

        if nb_images % TILES_PER_ROW == 0:
            self._nb_rows = nb_images // TILES_PER_ROW
        else:
            self._nb_rows = nb_images // TILES_PER_ROW + 1

        self._concat_image_width = images_width * TILES_PER_ROW
        self._concat_image_height = images_height * self._nb_rows
        self._mode = mode
        self._concat_image = Image.new(
            mode,
            (
                self._concat_image_width,
                self._concat_image_height
            )
        )

    def add_image(self, image: Image.Image, index: int):
        self._concat_image.paste(
            image,
            (
                image.width * (index % TILES_PER_ROW),
                image.height * (index // TILES_PER_ROW)
            )
        )

    def get_result(self) -> Image.Image:
        return self._concat_image


# ==================================== MAIN ===================================


if __name__ == "__main__":

    for directory in DIRECTORIES_TO_PARSE:
        files = glob.glob(directory + "/*.png")
        sample_file: Image.Image = open_image(files[0])

        sheet: AnimationSheetGenerator = AnimationSheetGenerator(
            len(files),
            sample_file.width,
            sample_file.height,
            sample_file.mode
        )

        for i in range(len(files)):
            print(f"Processing {files[i]}")
            sheet.add_image(open_image(files[i]), i)

        filepath: str = os.path.join(
            OUTPUT_DIR,
            f"{OUTPUT_FILES_NAME}_{os.path.split(directory)[1]}.{OUTPUT_EXTENSION}"
        )

        print(f"Saving to {filepath}")
        sheet.get_result().save(filepath)

    print("Done")
